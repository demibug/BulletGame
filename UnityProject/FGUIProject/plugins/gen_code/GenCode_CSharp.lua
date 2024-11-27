local function genCode(handler)
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath..'/'..codePkgName
    local namespaceName = codePkgName
    
    if settings.packageName~=nil and settings.packageName~='' then
        namespaceName = settings.packageName..'.'..namespaceName;
    end

    --CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname, nil)
    handler:SetupCodeFolder(exportCodePath, "cs") --check if target folder exists, and delete old files

    local getMemberByName = settings.getMemberByName

    local classCnt = classes.Count
    local writer = CodeWriter.new()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        local members = classInfo.members
        writer:reset()

        writer:writeln('using Cysharp.Threading.Tasks;')
        writer:writeln('using FairyGUI;')
        writer:writeln('using FairyGUI.Utils;')
        writer:writeln()
        writer:writeln('namespace %s', namespaceName)
        writer:startBlock()
        writer:writeln('public class %s : %s', classInfo.className, classInfo.superClassName)
        writer:startBlock()

		writer:writeln('public const string UIPackageName = "%s";', handler.pkg.name)
		writer:writeln('public const string UIResName = "%s";', classInfo.resName)
        writer:writeln()
        local memberCnt = members.Count
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            writer:writeln('public %s %s;', memberInfo.type, memberInfo.varName)
        end
        writer:writeln('public const string URL = "ui://%s%s";', handler.pkg.id, classInfo.resId)
        writer:writeln()

        writer:writeln('public static %s CreateInstance()', classInfo.className)
        writer:startBlock()
        writer:writeln('return (%s)UIPackage.CreateObject(UIPackageName, UIResName);', classInfo.className)
        writer:endBlock()
        writer:writeln()
		
		writer:writeln('public static UniTask<%s> CreateInstanceAsync()', classInfo.className)
        writer:startBlock()
        writer:writeln('UniTaskCompletionSource<%s> tcs = new UniTaskCompletionSource<%s>();', classInfo.className, classInfo.className)
        writer:writeln()
		writer:writeln('CreateGObjectAsync((go) =>')
        writer:startBlock()
        writer:writeln('tcs.TrySetResult((%s)go);', classInfo.className)
        writer:endBlock()
        writer:writeln(');')
        writer:writeln()
        writer:writeln('return tcs.Task;')
        writer:endBlock()
        writer:writeln()
		
		writer:writeln('private static void CreateGObjectAsync(UIPackage.CreateObjectCallback callback)')
        writer:startBlock()
        writer:writeln('UIPackage.CreateObjectAsync(UIPackageName, UIResName, callback);')
        writer:endBlock()
        writer:writeln()

        if handler.project.type==ProjectType.MonoGame then
            writer:writeln("protected override void OnConstruct()")
            writer:startBlock()
        else
            writer:writeln('public override void ConstructFromXML(XML xml)')
            writer:startBlock()
            writer:writeln('base.ConstructFromXML(xml);')
            writer:writeln()
        end
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            if memberInfo.group==0 then
                if getMemberByName then
                    writer:writeln('%s = (%s)GetChild("%s");', memberInfo.varName, memberInfo.type, memberInfo.name)
                else
                    writer:writeln('%s = (%s)GetChildAt(%s);', memberInfo.varName, memberInfo.type, memberInfo.index)
                end
            elseif memberInfo.group==1 then
                if getMemberByName then
                    writer:writeln('%s = GetController("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = GetControllerAt(%s);', memberInfo.varName, memberInfo.index)
                end
            else
                if getMemberByName then
                    writer:writeln('%s = GetTransition("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = GetTransitionAt(%s);', memberInfo.varName, memberInfo.index)
                end
            end
        end
        writer:endBlock()

        writer:endBlock() --class
        writer:endBlock() --namepsace

        writer:save(exportCodePath..'/'..classInfo.className..'.cs')
    end

    writer:reset()
	
	
    --local classCnt = classes.Count
    --local writer = CodeWriter.new()
    --for i=0,classCnt-1 do
    --    local classInfo = classes[i]
    --    local members = classInfo.members
    --    writer:reset()
	--
    --    writer:writeln('using FairyGUI;')
    --    writer:writeln('using GameFgui;')
    --    writer:writeln()
    --    writer:writeln('namespace %s', namespaceName)
    --    writer:startBlock()
    --    writer:writeln('public partial class %s : IEUIBase', 'E'..classInfo.className)
    --    writer:startBlock()
	--
    --    local memberCnt = members.Count
    --    for j=0,memberCnt-1 do
    --        local memberInfo = members[j]
    --        writer:writeln('public %s %s;', memberInfo.type, memberInfo.varName)
    --    end
	--	
    --    writer:writeln()
	--	writer:writeln('public void ConstructObject(GComponent rootComp)')
	--	writer:startBlock()
	--	
    --    for j=0,memberCnt-1 do
    --        local memberInfo = members[j]
    --        if memberInfo.group==0 then
    --            if getMemberByName then
    --                writer:writeln('%s = (%s)rootComp?.GetChild("%s");', memberInfo.varName, memberInfo.type, memberInfo.name)
    --            else
    --                writer:writeln('%s = (%s)rootComp?.GetChildAt(%s);', memberInfo.varName, memberInfo.type, memberInfo.index)
    --            end
    --        elseif memberInfo.group==1 then
    --            if getMemberByName then
    --                writer:writeln('%s = rootComp?.GetController("%s");', memberInfo.varName, memberInfo.name)
    --            else
    --                writer:writeln('%s = rootComp?.GetControllerAt(%s);', memberInfo.varName, memberInfo.index)
    --            end
    --        else
    --            if getMemberByName then
    --                writer:writeln('%s = rootComp?.GetTransition("%s");', memberInfo.varName, memberInfo.name)
    --            else
    --                writer:writeln('%s = rootComp?.GetTransitionAt(%s);', memberInfo.varName, memberInfo.index)
    --            end
    --        end
    --    end
    --    writer:endBlock()
	--
    --    writer:endBlock() --class
    --    writer:endBlock() --namepsace
	--
    --    writer:save(exportCodePath..'/E'..classInfo.className..'.cs')
    --end

    writer:reset()
	

    local binderName = codePkgName..'Binder'

    writer:writeln('using FairyGUI;')
    writer:writeln()
    writer:writeln('namespace %s', namespaceName)
    writer:startBlock()
    writer:writeln('public class %s', binderName)
    writer:startBlock()

	writer:writeln('private static int s_binderId;')
	writer:writeln('public static int BinderId')
    writer:startBlock()
	writer:writeln('get')
	writer:startBlock()
	writer:writeln('if (s_binderId == 0) s_binderId = TEngine.RuntimeId.ToRuntimeId("%s");', binderName)
	writer:writeln('return s_binderId;')
	writer:endBlock() -- binder id get
	writer:endBlock() -- binder id
	writer:writeln()
    writer:writeln('public static void BindAll()')
    writer:startBlock()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        writer:writeln('UIObjectFactory.SetPackageItemExtension(%s.URL, typeof(%s));', classInfo.className, classInfo.className)
    end
    writer:endBlock() --bindall

    writer:endBlock() --class
    writer:endBlock() --namespace
    
    writer:save(exportCodePath..'/'..binderName..'.cs')
end

return genCode