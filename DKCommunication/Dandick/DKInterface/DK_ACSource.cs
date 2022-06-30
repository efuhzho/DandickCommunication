using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface DK_ACSource
    {
        /// <summary>
        /// 指示是否具有交流电压输出功能
        /// </summary>
        bool IsACU_Activated { get; set; }

        /// <summary>
        /// 指示是否具有交流电流输出功能
        /// </summary>
        bool IsACI_Activated { get; set; }

        /// <summary>
        /// 源关闭命令
        /// </summary>
        void Stop( );

        /// <summary>
        /// 源打开命令
        /// </summary>
        void Start( );

        /// <summary>
        /// 设置档位命令
        /// </summary>
        void SetRange( );

        /// <summary>
        /// 设置输出幅度命令
        /// </summary>
        void WriteAmplitude( );
        void WritePhase( );
        void WriteFrequency( );
        void SetWireMode( );
        void SetClosedLoop( );
        void WriteHarmonics( );
    }
}
