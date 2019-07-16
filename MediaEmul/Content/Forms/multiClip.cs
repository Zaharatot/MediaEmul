using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaEmul.Content.Controls;

namespace MediaEmul.Content.Forms
{
    /// <summary>
    /// Форма отображения виртуального буфера обмена
    /// </summary>
    public partial class multiClip : Form
    {
        /// <summary>
        /// Количество виртуальных буферов обмена
        /// </summary>
        private const int countClipboards = 9;

        /// <summary>
        /// Список строк с данными из буфера обмена
        /// </summary>
        private ClipboardRow[] rows;

        /// <summary>
        /// Флаг запрета закрытия формы
        /// </summary>
        public bool disallowExit;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public multiClip()
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
            //Закрытие формы запрещено по дефолту
            disallowExit = true;
            //Инициализируем строки с результатами из буфера обмена
            initRows(); 
            //Инициализируем обработчики событий
            initEvents();
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void initEvents()
        {
            //Добавляем обработчик закрытия формы
            this.FormClosing += MultiClip_FormClosing;
        }

        /// <summary>
        /// Инициализируем строки с результатами из буфера обмена
        /// </summary>
        private void initRows()
        {
            //Инициализируем строки буфера обмена
            rows = new ClipboardRow[countClipboards + 1];
            //Проходимся по всем буферам обмена
            for (int i = countClipboards; i >= 0; i--)
                //Добавляем новую строку
                rows[i] = addRow(i);
        }

        /// <summary>
        /// Добавляем новую строку
        /// </summary>
        /// <param name="id">Id строки</param>
        /// <returns>Строка</returns>
        private ClipboardRow addRow(int id)
        {
            //Инициализируем новую строку
            ClipboardRow ex = new ClipboardRow();
            //Проставляем параметры
            ex.Dock = DockStyle.Top;            
            ex.value = "";
            ex.RowId = id;
            //Добавляем обработчик события клика по строке
            ex.onRowClick += Ex_onRowClick;
            //Добавляем троку на форму
            this.Controls.Add(ex);
            //Возвращаем результат
            return ex;
        }

        /// <summary>
        /// Обработчик события клика по строке
        /// </summary>
        /// <param name="rowId">Id строки, по которой был клик</param>
        private void Ex_onRowClick(int rowId)
        {

        }
        
        /// <summary>
        /// Обработчик закрытия формы
        /// </summary>
        private void MultiClip_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Сворачиваем форму
            this.Hide();
            //Отменяем закрытие формы
            e.Cancel = disallowExit;
        }

    }
}
