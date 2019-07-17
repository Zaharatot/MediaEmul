using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MediaEmul.Content.Clases.DataClases.enums;
using static MediaEmul.Content.Clases.DataClases.events;

namespace MediaEmul.Content.Clases.WorkClases
{
    /// <summary>
    /// Класс работы с буфером обмена
    /// </summary>
    class ClipboardWorker : IDisposable
    {
        /// <summary>
        /// Делей, между итерациями цикла основного потока
        /// </summary>
        private const int sleepDelay = 30;

        /// <summary>
        /// Время, выделяемое на двойной клик
        /// </summary>
        private const int doubleClickDelay = 250;


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
        /// Беферы обмена
        /// </summary>
        private string[] clipboards;
        /// <summary>
        /// Количество виртуальных буферов обмена
        /// </summary>
        private const int countClipboards = 9;

        /// <summary>
        /// Значение буфера, для сохранения
        /// </summary>
        private object bufferVal;
        /// <summary>
        /// Тип данных взятых из буфера обмена
        /// </summary>
        private clipboardDataType buffType;

        /// <summary>
        /// Список кнопок с цифрами
        /// </summary>
        private List<Keys> numberKeys;

        /// <summary>
        /// Класс эмулятора нажатия клавишь
        /// </summary>
        private EmulatorWorker ew;

        /// <summary>
        /// Флаг работы
        /// </summary>
        private bool onWork;
        /// <summary>
        /// Основной рабочий поток класса
        /// </summary>
        private Thread main;

        /// <summary>
        /// Переменная для отслеживания двойного нажатия кнопки
        /// </summary>
        private int oldDown;
        /// <summary>
        /// Время последнего нажатия клавиши
        /// </summary>
        private long oldDownTime;

        /// <summary>
        /// Id буфера, для копирования в него
        /// </summary>
        private int copyId;
        /// <summary>
        /// Id буфера, для вставки в него
        /// </summary>
        private int pasteId;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="ew">ссылка на класс эмулятора нажатия клавишь</param>
        public ClipboardWorker(EmulatorWorker ew)
        {
            //Сохраняем ссылку на класс эмулятора нажатия клавишь
            this.ew = ew;
            //Инициализируем класс
            init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void init()
        {
            //Инициализируем переменные
            initVariables();
            //Инициализируем основной рабочий поток
            main = new Thread(work);
            //Указываем, что потом может работать с таймером
            main.ApartmentState = ApartmentState.STA;
            //Запускаем основной рабочий поток
            main.Start();
        }

        /// <summary>
        /// Инициализируем переменные
        /// </summary>
        private void initVariables()
        {
            //Указываем, что нажатий не было
            oldDownTime = oldDown = -1;

            //Ни вставки ни копирования нет
            copyId = pasteId = -1;

            //Инициализируем список буферов обмена
            clipboards = new string[countClipboards];
            //Инициализируем массив пустыми значениями
            for (int i = 0; i < countClipboards; i++)
                clipboards[i] = "";

            //Инициализируем список цифровых клавишь
            numberKeys = new List<Keys>() {
                Keys.D1,
                Keys.D2,
                Keys.D3,
                Keys.D4,
                Keys.D5,
                Keys.D6,
                Keys.D7,
                Keys.D8,
                Keys.D9
            };

            //По дефолту - работа идёт
            onWork = true;
        }

        /// <summary>
        /// Бекапим значение буфера обмена
        /// </summary>
        private void backupBufferValue()
        {
            try
            {

                //Если у нас аудиопоток
                if (Clipboard.ContainsAudio())
                {
                    //Сохраняем тип значения
                    buffType = clipboardDataType.Аудиопоток;
                    //Сохраняем значение буфера обмена
                    bufferVal = Clipboard.GetAudioStream();
                }
                //Если у нас файлы
                else if (Clipboard.ContainsFileDropList())
                {
                    //Сохраняем тип значения
                    buffType = clipboardDataType.Файлы;
                    //Сохраняем значение буфера обмена
                    bufferVal = Clipboard.GetFileDropList();
                }
                //Если у нас картинка
                else if (Clipboard.ContainsImage())
                {
                    //Сохраняем тип значения
                    buffType = clipboardDataType.Картинка;
                    //Сохраняем значение буфера обмена
                    bufferVal = Clipboard.GetImage();
                }
                //Если у нас текст
                else if (Clipboard.ContainsText())
                {
                    //Сохраняем тип значения
                    buffType = clipboardDataType.Текст;
                    //Сохраняем значение буфера обмена
                    bufferVal = Clipboard.GetText();
                }
                //В остальных случаях
                else
                    //Очищаем буфер
                    bufferVal = null;
            }
            catch { }
        }

        /// <summary>
        /// Восстанавливаем значение буфера обмена
        /// </summary>
        private void restoreBufferValue()
        {
            try
            {
                //Если есть бекап значения
                if (bufferVal != null)
                {
                    //Выбираем как втыкать по типу
                    switch(buffType)
                    {
                        case clipboardDataType.Аудиопоток:
                            {
                                //Втыкаем аудио
                                Clipboard.SetAudio((Stream)bufferVal);
                                break;
                            }
                        case clipboardDataType.Картинка:
                            {
                                //Втыкаем картинку
                                Clipboard.SetImage((Image)bufferVal);
                                break;
                            }
                        case clipboardDataType.Текст:
                            {
                                //Втыкаем текст
                                Clipboard.SetText((string)bufferVal);
                                break;
                            }
                        case clipboardDataType.Файлы:
                            {
                                //Втыкаем файлы
                                Clipboard.SetFileDropList((StringCollection)bufferVal);
                                break;
                            }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Получаем текст из буфера обмена
        /// </summary>
        /// <returns>Текст из буфера</returns>
        private string getClipboardText()
        {
            string ex = "";

            try
            {
                //Если текст в буфере есть
                if (Clipboard.ContainsText())
                    //Получаем его
                    ex = Clipboard.GetText();
            }
            catch { ex = ""; }

            return ex;
        }

        /// <summary>
        /// Вставляем текст в буфер обмена
        /// </summary>
        /// <param name="text">Новый текст для буфера</param>
        private void setClipboardText(string text)
        {
            try
            {
                //Втыкаем текст в буфер обмена
                Clipboard.SetText(text);
            }
            catch { }
        }

        /// <summary>
        /// Получаем значение буфера обмена
        /// </summary>
        /// <param name="id">Id буфера</param>
        /// <returns>Текст из буфера обмена</returns>
        public string getClipboardValue(int id)
        {
            string ex = "";

            try
            {
                //Если id = -1, 
                if (id == -1)
                    //то берём значение основного буфера
                    ex = getClipboardText();
                //В остальных случаях
                else if (id < countClipboards)
                    //Возвращаем текст нашего буфера
                    ex = clipboards[id];
            }
            catch { ex = ""; }

            return ex;
        }

        /// <summary>
        /// Записываем в наш буфер обмена значение из основного
        /// </summary>
        /// <param name="id">Id буфера</param>
        /// <returns>Вставленный текст</returns>
        private string setValue(int id)
        {
            string ex = "";
            try
            {
                //Если id находится в пределах диапазона
                if ((id >= 0) && (id < countClipboards))
                {
                    //Вписываем в буфер с таким id значение из буфера обмена
                    clipboards[id] = getClipboardText();
                    //Сохраняем полученный текст для воз-вращаения
                    ex = clipboards[id];
                }
            }
            catch { ex = ""; }

            return ex;
        }

        /// <summary>
        /// Переносим значение из нашего буфера обмена в основной
        /// </summary>
        /// <param name="id">Id буфера</param>
        private void getValue(int id)
        {
            try
            {
                //Если id находится в пределах диапазона
                if ((id >= 0) && (id < countClipboards))
                    //Вписываем в буфер с таким id значение из буфера обмена
                    setClipboardText(clipboards[id]);
            }
            catch { }
        }

        /// <summary>
        /// Копируем текст в наш буфер обмена
        /// </summary>
        /// <param name="id">id буфера обмена</param>
        private void copyToBuffer(int id)
        {
            string text;
            try
            {
                //Бекапим текущее значение буфера
                backupBufferValue();
                //Эмулируем нажатие сочетания Ctrl+C
                ew.emulateCopy();
                //Записываем полученное значение в наш буфер обмена
                text = setValue(id);
                //Восстанавливаем старое значение буфера
                restoreBufferValue();
                //Вызываем ивент обновления буфера обмена
                onUpdateBuffer?.Invoke(text, id);
            }
            catch { }
        }

        //TODO: Короче говоря, периодически копирование/вставка не срабатывают нормально.
        //При этом, текст из буфера замещается на скопированный.
        //т.е. ctrl+v отработать успевает (ну, или втыкание в буфер)
        //а вставка и замена - нет.
        //Нужно добавлять логирование на ошибки и думать.

        /// <summary>
        /// Вставляем текст из нашего буфера обмена
        /// </summary>
        /// <param name="id">id буфера обмена</param>
        private void pasteFromBuffer(int id)
        {
            try
            {
                //Стираем нажатые 2 раза кнопки
                ew.clearDoubleCkickResult();
                //Бекапим текущее значение буфера
                backupBufferValue();
                //Переносим значение из нашего буфера обмена в основной
                getValue(id);
                //Эмулируем нажатие сочетания Ctrl+V
                ew.emulatePaste();
                //Восстанавливаем старое значение буфера
                restoreBufferValue();
            }
            catch { }
        }

        /// <summary>
        /// Устанавливаем значение буфера обмена, при 
        /// нажатии необходимых сочетаний клавишь
        /// </summary>
        /// <param name="key">Нажатая кнопка</param>
        /// <param name="control">Флаг нажатия кнопки Ctrl</param>
        public void setClipboard(Keys key, bool control)
        {
            int value;

            //Если нажато сочетание Ctrl+0
            if (control && (key == Keys.D0))
                //Отображаем форму буферов обмена
                onShowbufferForm?.Invoke();
            //В противном случае
            else
            {
                //Получаем цифру нажатой клавиши
                value = getNumberByKey(key) - 1;

                //При нажатой кнопке ctrl
                if (control)
                {
                    //Если была нажата цифровая кнопка
                    if (numberKeys.Contains(key))
                        //Указываем, что нужно выполнить копирование
                        copyId = value;
                }
                else
                {
                    //Если была нажата цифровая кнопка
                    if (numberKeys.Contains(key))
                    {
                        //Если предыдущего нажатия не было
                        if (oldDown == -1)
                        {
                            //Запоминаем это нажатие
                            oldDown = value;
                            //И запоминаем его время
                            oldDownTime = timeMicro();
                        }
                        //Если нажатие было, и оно совпадает с текущим, и
                        //при этом, между ними прошло не более установленного времени
                        else if ((oldDown == value) && (timeMicro() - oldDownTime < doubleClickDelay))
                        {
                            //Сбрасываем значения
                            oldDownTime = oldDown = -1;
                            //запоминаем задачу вставки
                            pasteId = value;
                        }
                        //В противном случае
                        else
                            //Сбрасываем значения
                            oldDownTime = oldDown = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Получаем цифру на кнопке из кода клавиши
        /// </summary>
        /// <param name="key">Код клавиши</param>
        /// <returns>Цифра на кнопке</returns>
        private int getNumberByKey(Keys key) =>
            (int)key - 48;
        

        /// <summary>
        /// Класс основного потока
        /// </summary>
        private void work()
        {
            do
            {
                //Проверяем наличие задач клпирования/вставки и выполняем их
                checkCopyPaste();

                //Спим положенное время
                Thread.Sleep(sleepDelay);
            }
            //Цикл идёт, пока идёт работа
            while (onWork);
        }

        /// <summary>
        /// Проверяем наличие задач клпирования/вставки и выполняем их
        /// </summary>
        private void checkCopyPaste()
        {
            //Если нужно скопировать в буфер значение
            if (copyId != -1)
            {
                //Копируем текст в нужный буфер обмена
                copyToBuffer(copyId);
                //Сбрасываем задачу
                copyId = -1;
            }
            //Если нужно вставить значение из буфера
            if(pasteId != -1)
            {
                //Вставляем текст из указанного буфера
                pasteFromBuffer(pasteId);
                //Сбрасываем задачу
                pasteId = -1;
            }
        }

        /// <summary>
        /// Получаем время в милисекундах
        /// </summary>
        /// <returns>Дабловое число секунд</returns>
        public static long timeMicro()
        {
            return (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 1000);
        }

        /// <summary>
        /// Очищаем ресурсы используемые классом
        /// </summary>
        public void Dispose()
        {
            //Останавливаем работу
            onWork = false;
            //Если поток существует и работает
            if ((main != null) && (main.IsAlive))
                //Прерываем его работу
                main.Abort();
        }
    }
}
