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
        /// Класс хука клавиатуры
        /// </summary>
        private KeyboardHook kh;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public EmulatorWorker()
        {

            //Инициализация глобального хука клавиатуры
            kh = new KeyboardHook(true);
            //Добавляем события для хука
            kh.KeyDown += Kh_KeyDown;
        }


        /// <summary>
        /// Событие нажатия кнопки клавиатуры 
        /// </summary>
        private void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            //Если нажат контрл
            if (Ctrl)
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
        }

        /// <summary>
        /// Эмулируем нажатия клавиши
        /// </summary>
        /// <param name="keyCode">Код клавиши</param>
        public void emul(byte keyCode)
        {
            keybd_event(keyCode, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(keyCode, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }


        /// <summary>
        /// Очищаем ресурсы класса
        /// </summary>
        public void Dispose()
        {
            //Если хук клавиатуры инициализирован
            if (kh != null)
                //Очищаем его
                kh.Dispose();
        }
    }
}
