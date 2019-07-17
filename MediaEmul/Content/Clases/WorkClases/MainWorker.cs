using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MediaEmul.Content.Clases.DataClases.events;

namespace MediaEmul.Content.Clases.WorkClases
{
    /// <summary>
    /// Основной рабочий класс
    /// </summary>
    public class MainWorker : IDisposable
    {

        /// <summary>
        /// Событие обновления буфера
        /// </summary>
        public event updateBuffer onUpdateBuffer;
        /// <summary>
        /// Событие запроса отображения 
        /// формы со списком буферов
        /// </summary>
        public event showbufferForm onShowbufferForm;

        /// <summary>
        /// Класс хука клавиатуры
        /// </summary>
        private KeyboardHook kh;
        /// <summary>
        /// Класс эмулятора медиаклавишь
        /// </summary>
        private EmulatorWorker ew;
        /// <summary>
        /// Класс работы с буфером обмена
        /// </summary>
        private ClipboardWorker cw;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainWorker()
        {
            //Инициализируем калсс
            init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void init()
        {
            //Инициализируем эмулятор нажатия клавишь
            ew = new EmulatorWorker();
            //Инициализируем класс работы с буфером обмена
            cw = new ClipboardWorker(ew);

            //Добавляем обработчик события обновления буфера обмена
            cw.onUpdateBuffer += Cw_onUpdateBuffer;
            //Добавляем обработчик события вызова формы буферов
            cw.onShowbufferForm += Cw_onShowbufferForm;

            //Инициализация глобального хука клавиатуры
            kh = new KeyboardHook(true);
            //Добавляем события для хука
            kh.KeyDown += Kh_KeyDown;
        }

        /// <summary>
        /// Обработчик события вызова формы буферов
        /// </summary>
        private void Cw_onShowbufferForm()
        {
            //Вызываем внешнее событие
            onShowbufferForm?.Invoke();
        }

        /// <summary>
        /// Обработчик события обновления буфера обмена
        /// </summary>
        /// <param name="text">Текст буфера</param>
        /// <param name="id">Id буфера</param>
        private void Cw_onUpdateBuffer(string text, int id)
        {
            //Вызываем внешнее событие
            onUpdateBuffer?.Invoke(text, id);
        }

        /// <summary>
        /// Событие нажатия кнопки клавиатуры 
        /// </summary>
        private void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            //Если нажат контрл
            if (Ctrl)
            {
                //Выбираем медиадействие по нажатой клавише
                ew.setEmulateKey(key);
            }

            //Выбираем взаимодействие с буфером обмена по нажатой клавише
            cw.setClipboard(key, Ctrl);
        }

       


        /// <summary>
        /// Очищаем ресурсы, используемые классом
        /// </summary>
        public void Dispose()
        {
            //Если эмулятор был инициализован
            if (ew != null)
                //Закрываем эмулятор
                ew.Dispose();
            //Если хук клавиатуры инициализирован
            if (kh != null)
                //Очищаем его
                kh.Dispose();
            //Если работа с буфером была инициализирована
            if (cw != null)
                //Очищаем её
                cw.Dispose();
        }
    }
}
