using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaEmul.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс эмуляции медиаклавишь
    /// </summary>
    public class EmulatorWorker : IDisposable
    {
        /// <summary>
        /// Уонстанта кода нажатия кнопки
        /// </summary>
        const int KEYEVENTF_EXTENDEDKEY = 0x1;
        /// <summary>
        /// Уонстанта кода отпускания кнопки
        /// </summary>
        const int KEYEVENTF_KEYUP = 0x2;

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);



        /// <summary>
        /// Конструктор класса
        /// </summary>
        public EmulatorWorker()
        {
        }

        /// <summary>
        /// Выбираем тип действия, по нажатой клавиши
        /// </summary>
        /// <param name="key">Код нажатой клавиши</param>
        public void setEmulateKey(Keys key)
        {
            switch (key)
            {
                case Keys.Enter:
                    {
                        //Ставим трек на паузу
                        emul((int)Keys.MediaPlayPause);
                        break;
                    }
                case Keys.Add:
                    {
                        //Включаем следующий трек
                        emul((int)Keys.MediaNextTrack);
                        break;
                    }
                case Keys.Subtract:
                    {
                        //Включаем предыдущий трек
                        emul((int)Keys.MediaPreviousTrack);
                        break;
                    }
            }
        }


        /// <summary>
        /// Эмулируем нажатия клавиши
        /// </summary>
        /// <param name="keyCode">Код клавиши</param>
        private void emul(byte keyCode)
        {
            //Эмулируем нажатие и отпускание кнопки
            keybd_event(keyCode, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(keyCode, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }


        /// <summary>
        /// Эмулируем нажатие сочетания Ctrl+C
        /// </summary>
        public void emulateCopy()
        {
            //Нажимаем кнопку 'Ctrl'
            keybd_event((byte)Keys.ControlKey, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            //Нажимаем и отпускаем кнопку 'C'
            keybd_event((byte)Keys.C, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event((byte)Keys.C, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            //Отпускаем кнопку 'Ctrl'
            keybd_event((byte)Keys.ControlKey, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }


        /// <summary>
        /// Эмулируем нажатие сочетания Ctrl+V
        /// </summary>
        public void emulatePaste()
        {
            //Нажимаем кнопку 'Ctrl'
            keybd_event((byte)Keys.ControlKey, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            //Нажимаем и отпускаем кнопку 'V'
            keybd_event((byte)Keys.V, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event((byte)Keys.V, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            //Отпускаем кнопку 'Ctrl'
            keybd_event((byte)Keys.ControlKey, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// Очищаем результат двойного клика
        /// </summary>
        public void clearDoubleCkickResult()
        {
            //Нажимаем и отпускаем кнопку 'Backspase', 2 раза
            keybd_event((byte)Keys.Back, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event((byte)Keys.Back, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            keybd_event((byte)Keys.Back, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event((byte)Keys.Back, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// Очищаем ресурсы класса
        /// </summary>
        public void Dispose()
        {
        }
    }
}
