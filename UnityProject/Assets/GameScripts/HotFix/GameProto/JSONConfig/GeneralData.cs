namespace GameConfig 
{
    namespace General
	{
		public partial class GeneralData
		{
			public int[][] InitialItems;				//初始化物品 二维数组(第一个棋子或护盾为默认初始装备)
			public int InitialMap;				//初始化场景编号 int
			public int[] ChessSelection;				//选择棋子配置（棋子ID）
			public int[] NameLength;				//名字长度[最小字符，最大字符]
			public int ChangeNameNum;				//最大改名次数
			public int[] RandomHead;				//初始随机头像
			public int DiceMultiplierUnlock;				//骰子倍数设置解锁条件[棋盘编号]
			public int DiceRecoverTime;				//骰子每X秒恢复的骰子数
			public int MaxHousesNumber;				//地产最大房子数
			public int Houses;				//建筑建造或升1级增加小房子数
			public int dailytask_update;				//每日任务重置时间（与UTC相差，+-）
			public int Shield_quantity;				//棋盘上防护罩数量
			public int[] Shield_position;				//初始防护罩棋盘位置[第1个防护罩位置,第2个防护罩位置1,第3个防护罩位置1,第4个防护罩位置,]
			public int DailyPointRewards_update;				//积分任务更新时间（周一~周日）
			public int Rest_time_position;				//休息时刻停留棋格位置(休息时刻获得停留的位置)
			public int Lamp_open_time;				//神灯开启时间（与UTC相差，+-）
			public int[] coin_effects;				//金币抛洒特效表现配置[抛金币特效的金币倍数条件,数量倍数,最小金币数，最大金币数]
			public int[] dice_effects;				//骰子抛洒特效表现配置[抛骰子特效的骰子倍数条件,数量倍数,最小骰子数，最大骰子数]
			public int NetAssets;				//地产触发色系转盘每个地产获得的净资产值
			public int TargetWeight;				//租金换目标权重（不换目标权重=100-换目标权重）
			public int FriendWeight;				//租金目标选择好友的权重（非好友权重=1-好友权重）
			public int MinigameClearTime;				//小游戏掉线清除时间
			public int GameTime;				//外网测试版本游戏时长(秒)
			public int action_power;				//异次元行动力上限
			public int EmojiSlotImgInterval;				//轮播图片间隔时间（毫秒）
			public int time_zone_offset;				//时区偏移（UTC时间，整数）
			public int auto_loading_time;				//自动掷骰子读条时间（毫秒）
		}

	}
}
