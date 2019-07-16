using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaEmul.Content.Clases.WorkClases;

namespace MediaEmul.Content.Forms
{
    /// <summary>
    /// Основная форма приложения
    /// </summary>
    public partial class main : Form
    {
        /// <summary>
        /// Форма мульти буфера обмена
        /// </summary>
        private multiClip mcf;


        /// <summary>
        /// Класс эмулятора медиаклавишь
        /// </summary>
        private EmulatorWorker ew;

        /// <summary>
        /// Флаг запрета закрытия формы
        /// </summary>
        private bool disallowExit;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public main()
        {
            //Инициализируем форму
            InitializeComponent();
            //Инициализируем класс
            init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void init()
        {
            //Инициализируем форму мульти буфера обмена
            mcf = new multiClip();

            //Инициализируем эмуляцию медиаклавишь
            ew = new EmulatorWorker();

            //Закрытие формы запрещено по дефолту
            disallowExit = true;

            //Инициализируем обработчики событий
            initEvents();
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void initEvents()
        {
            //Добавляем обработчик закрытия формы
            this.FormClosing += Main_FormClosing;
            //Добавляем обработчик завершения закрытия формы
            this.FormClosed += Main_FormClosed;
            //Добавляем обработчик клика по иконке в трее
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        /// <summary>
        /// Обработчик завершения закрытия формы
        /// </summary>
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Если эмулятор был инициализован
            if(ew != null)
                //Закрываем эмулятор
                ew.Dispose();
        }

        /// <summary>
        /// Обработчик клика по иконке в трее
        /// </summary>
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            //Разворачиваем форму
            this.Show();
        }

        /// <summary>
        /// Обработчик закрытия формы
        /// </summary>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Сворачиваем форму
            this.Hide();
            //Отменяем закрытие формы
            e.Cancel = disallowExit;
        }


        /// <summary>
        /// Клик по пункту "Выход" в меня иконки в трее
        /// </summary>
        private void notifyExit_Click(object sender, EventArgs e)
        {
            //Разрешаем закрытие форм
            mcf.disallowExit = disallowExit = false;            
            //Закрываем форму
            this.Close();
        }

        /// <summary>
        /// Клик по пункту "Мульти буфер обмена" в меня иконки в трее
        /// </summary>
        private void notifyMultiClip_Click(object sender, EventArgs e)
        {
            //Отображаем форму мульти буфера обмена
            mcf.Show();
        }
    }
}
