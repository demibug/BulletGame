using GameBase;

namespace GameAudio
{
    public enum SoundType
    {
        None = 0,
        BackGround = 1,
    }

    public class AudioSystem : Singleton<AudioSystem>
    {
        private SoundType curSoundType;
        private SoundType SaveSoundType;
        private bool isBackGroundPlay = false;
        public bool IsBackGroundPlay() => isBackGroundPlay;

        /// <summary>
        /// 是否可以播放背景音乐
        /// </summary>
        public bool IsCanPlayBgMusic { get; set; } = true;

        /// <summary>
        /// 是否可以播放音效
        /// </summary>
        public bool IsCanPlaySound {  get; set; } = true;


        /// <summary>
        /// 复位设定（每次重新登陆都要进行的事件）
        /// </summary>
        public void ResetSetting()
        {
            IsCanPlayBgMusic = true;
            IsCanPlaySound = true;
        }

        //private Dictionary<string, AudioAgent> dic_audio = new Dictionary<string, AudioAgent>();

        /// <summary>
        /// 获取音效配置
        /// </summary>
        /// <param name="audioId"></param>
        /// <returns></returns>
        // public SoundClass GetAudioConfig(int audioId)
        // {
        //     Sound sound = ConfigSystem.Instance.Tables.Sound;
        //     return sound.GetOrDefault(audioId);
        // }
        //
        // /// <summary>
        // /// 播放之前的背景音效
        // /// </summary>
        // public void PlaySound_BeforeBackGround()
        // {
        //     if (SaveSoundType.Equals(SoundType.None))
        //     {
        //         PlaySound_BackGround();
        //     }
        //     else
        //     {
        //         if (!IsCanPlayBgMusic) return;
        //         if (curSoundType.Equals(SaveSoundType)) return;
        //         StopSound_BackGround();
        //         curSoundType = SaveSoundType;
        //         SaveSoundType = SoundType.None;
        //         SoundClass theSound = GetAudioConfig((int)curSoundType);
        //         if (!isBackGroundPlay && PlaySound(theSound, TEngine.AudioType.Music, true))
        //         {
        //             isBackGroundPlay = true;
        //         }
        //     }
        // }
        //
        // /// <summary>
        // /// 播放背景音效
        // /// </summary>
        // public void PlaySound_BackGround()
        // {
        //     if (!IsCanPlayBgMusic) return;
        //     if (curSoundType.Equals(SoundType.BackGround_New)) return;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.BackGround_New;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.BackGround_New);
        //     if (!isBackGroundPlay && PlaySound(theSound, TEngine.AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // /// <summary>
        // /// 停止背景音
        // /// </summary>
        // public void StopSound_BackGround()
        // {
        //     curSoundType = SoundType.None;
        //     //SoundClass theSound = GetAudioConfig((int)SoundType.BackGround);
        //     if (StopSound(AudioType.Music))
        //     {
        //         isBackGroundPlay = false;
        //     }
        // }
        //
        // /// <summary>
        // /// 破坏事件背景音效
        // /// </summary>
        // public void PlaySound_BreakDownBackGround()
        // {
        //     if (!IsCanPlayBgMusic) return;
        //     SaveSoundType = curSoundType;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.BreakDownBackGround;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.BreakDownBackGround);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // /// <summary>
        // /// 银行抢劫背景音效
        // /// </summary>
        // public void PlaySound_BankRobbery()
        // {
        //     if (!IsCanPlayBgMusic) return;
        //     SaveSoundType = curSoundType;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.BankRobbery;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.BankRobbery);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // /// <summary>
        // /// 小游戏背景音效
        // /// </summary>
        // public void PlaySound_LittleGameBg()
        // {
        //     if (!IsCanPlayBgMusic) return;
        //     SaveSoundType = curSoundType;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.LittleGame_Bg;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.LittleGame_Bg);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // /// <summary>
        // /// 小游戏背景音效
        // /// </summary>
        // public void PlaySound_LittleGame_BalloonFight_Bg()
        // {
        //     if (!IsCanPlayBgMusic) return;
        //     SaveSoundType = curSoundType;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.LittleGame_BalloonFight_Bg;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.LittleGame_BalloonFight_Bg);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // public void PlaySound_Aladdin_Bg()
        // {
        //     if (!IsCanPlayBgMusic) return;
        //     SaveSoundType = curSoundType;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.Aladdin_Bg;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.Aladdin_Bg);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // public void PlaySound_Buff_Snowstorm()
        // {
        //     if (curSoundType.Equals(SoundType.Buff_Snowstorm)) return;
        //     if (!IsCanPlayBgMusic) return;
        //     if(!ChkBuffSound(curSoundType)) SaveSoundType = curSoundType;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.Buff_Snowstorm;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.Buff_Snowstorm);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // public void PlaySound_Buff_Sandstorm()
        // {
        //     if (curSoundType.Equals(SoundType.Buff_Sandstorm)) return;
        //     if (!IsCanPlayBgMusic) return;
        //     if(!ChkBuffSound(curSoundType)) SaveSoundType = curSoundType;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.Buff_Sandstorm;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.Buff_Sandstorm);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        // //检测是否是BUFF背景音
        // private bool ChkBuffSound(SoundType soundType)
        // {
        //     if (curSoundType.Equals(SoundType.Buff_Snowstorm)
        //         || curSoundType.Equals(SoundType.Buff_Sandstorm))
        //     {
        //         return true;
        //     }
        //
        //     return false;
        // }
        //
        // /// <summary>
        // /// 导弹发射
        // /// </summary>
        // public void PlaySound_MissileFire()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.MissileFire);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 导弹击中
        // /// </summary>
        // public void PlaySound_FireAttacked()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.FireAttacked);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 直升机停留
        // /// </summary>
        // public void PlaySound_HelicopterStay()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.HelicopterStay);
        //     PlaySound(theSound, AudioType.Voice, true);
        // }
        // public void StopSound_HelicopterStay()
        // {
        //     curSoundType = SoundType.None;
        //     StopSound(AudioType.Voice, true);
        // }
        //
        // /// <summary>
        // /// 获得金币
        // /// </summary>
        // public void PlaySound_GetGold()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.GetGold);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 棋子跳跃
        // /// </summary>
        // public void PlaySound_ChessJump()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.ChessJump);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 投掷骰子音效
        // /// </summary>
        // public void PlaySound_DropChess()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     int random = UnityEngine.Random.Range((int)SoundType.DropChess, (int)SoundType.DropChess2 + 1);
        //     SoundClass theSound = GetAudioConfig(random);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 获得骰子
        // /// </summary>
        // public void PlaySound_GetChess()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //
        //     SoundClass theSound = GetAudioConfig((int)SoundType.GetChess);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 建筑升级
        // /// </summary>
        // public void PlaySound_Building()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //
        //     SoundClass theSound = GetAudioConfig((int)SoundType.Building);
        //     PlaySound(theSound);
        // }
        //
        //
        // /// <summary>
        // /// 飞机过场音效
        // /// </summary>
        // public void PlaySound_PlaneLoding()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.PlaneLoding);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 抢劫 打开宝箱
        // /// </summary>
        // public void PlaySound_BankOpenBox()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.BankOpenBox);
        //     PlaySound(theSound);
        // }
        //
        // /// <summary>
        // /// 抢劫 获得奖励
        // /// </summary>
        // public void PlaySound_BankGetReward()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.BankGetReward);
        //     PlaySound(theSound);
        // }
        // /// <summary>
        // /// 抢劫 获得大奖励
        // /// </summary>
        // public void PlaySound_BankGetBigReward()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.BankGetBigReward);
        //     PlaySound(theSound);
        // }
        // /// <summary>
        // /// 监狱音效
        // /// </summary>
        // public void PlaySound_Jail()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.Jail);
        //     PlaySound(theSound);
        // }
        // /// <summary>
        // /// 罚款
        // /// </summary>
        // public void PlaySound_Penalty()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.Penalty);
        //     PlaySound(theSound);
        // }
        // /// <summary>
        // /// 获取盾牌
        // /// </summary>
        // public void PlaySound_GetShield()
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.GetShield);
        //     PlaySound(theSound);
        // }
        //
        //
        // /// <summary>
        // /// 传送门背景音效
        // /// </summary>
        // public void PlaySound_AtherDoorBackGround()
        // {
        //     if (!IsCanPlayBgMusic) return;
        //     StopSound_BackGround();
        //     curSoundType = SoundType.AtherDoorBackGround;
        //     SoundClass theSound = GetAudioConfig((int)SoundType.AtherDoorBackGround);
        //     if (!isBackGroundPlay && PlaySound(theSound, AudioType.Music, true))
        //     {
        //         isBackGroundPlay = true;
        //     }
        // }
        //
        //
        //
        // /// <summary>
        // /// 根据类型获取音效
        // /// </summary>
        // public void PlaySound_ByType(SoundType type)
        // {
        //     if (!IsCanPlaySound)
        //         return;
        //     SoundClass theSound = GetAudioConfig((int)type);
        //     PlaySound(theSound);
        // }


        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="theSound">具体的音效配置 Sound.xlsx </param>
        // private bool PlaySound(SoundClass theSound, AudioType audioType = AudioType.Sound, bool bLoop = false)
        // {
        //     if (theSound != null && theSound.Resource != "")
        //     {
        //         //if (audioType.Equals(AudioType.Music) && dic_audio.ContainsKey(theSound.Resource))
        //         //{
        //         //    dic_audio[theSound.Resource].AudioResource().Play();
        //         //}
        //         //else
        //         //{
        //         //    var temp = GameModule.Audio.Play(audioType, theSound.Resource, bLoop);
        //         //    dic_audio[theSound.Resource] = temp;
        //         //}
        //         GameModule.Audio.Play(audioType, theSound.Resource, bLoop);
        //         return true;
        //     }
        //     return false;
        // }
        //
        // public bool StopSound(AudioType audioType,bool fadeout = false)
        // {
        //     //if (theSound != null)
        //     //{
        //     //    if (dic_audio.ContainsKey(theSound.Resource))
        //     //    {
        //     //        dic_audio[theSound.Resource].Stop();
        //     //        return true;
        //     //    }
        //     //}
        //     if (fadeout)
        //     {
        //         GameModule.Audio.Stop(audioType, fadeout);
        //         return true;
        //     }
        //
        //     GameModule.Audio.Stop(audioType, audioType == AudioType.Music ? true:false);
        //     return true;
        // }
    }
}
