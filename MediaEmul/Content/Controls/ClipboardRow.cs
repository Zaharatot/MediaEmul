using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaEmul.Content.Controls
{
    /// <summary>
    /// Контролл, реализующий строку с текстом из буфера обмена
    /// </summary>
    public partial class ClipboardRow : UserControl
    {
        /// <summary>
        /// Делегат события клика по контроллу
        /// </summary>
        /// <param name="rowId">Id строки, на которую кликнули</param>
        public delegate void rowClick(int rowId);
        /// <summary>
        /// Событие клика по контроллу
        /// </summary>
        public event rowClick onRowClick;

        /// <summary>
        /// Id данной строки
        /// </summary>
        private int id;

        /// <summary>
        /// Проставляем/получаем id теукщей строки
        /// </summary>
        public int RowId
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                //Проставляем красиво подсказку
                infoLabel.Text = $"Ctrl + {id + 1}";
            }
        }

        /// <summary>
        /// Текстовое значение строки
        /// </summary>
        public string value
        {
            get
            {
                return valueInfo.Text;
            }
            set
            {
                valueInfo.Text = (value.Length > 0) ? value : "Пусто...";
            }
        }

        /// <summary>
        /// Цвет выделения при наведении мыши
        /// </summary>
        public Color selectColor { get; set; }

        /// <summary>
        /// Конструктор контролла
        /// </summary>
        public ClipboardRow()
        {
            //Инициализация компонентив
            InitializeComponent();
            //Инициализация класса
            init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void init()
        {
            //Ставим дефолтный id, текст и цвет выделения
            id = 0;
            value = "";
            selectColor = Color.FromArgb(230, 230, 230);
            //Инициализируем обработчики событий
            initEvents();
        }

        /// <summary>
        /// Инициализируем обработчики событий
        /// </summary>
        private void initEvents()
        {
            //добавляем обработчик события наведения мыши на контролл
            infoLabel.MouseEnter += row_MouseEnter;
            valueInfo.MouseEnter += row_MouseEnter;
            //добавляем обработчик события убирания мыши с контролла
            infoLabel.MouseLeave += row_MouseLeave;
            valueInfo.MouseLeave += row_MouseLeave;

            //добавляем обработчик события клика по контроллу
            infoLabel.Click += row_Click;
            valueInfo.Click += row_Click;
        }

        /// <summary>
        /// Обработчик события клика по контроллу
        /// </summary>
        private void row_Click(object sender, EventArgs e)
        {
            //Вызываем внешнее событие
            onRowClick?.Invoke(id);
        }

        /// <summary>
        /// Обработчик события убирания мыши с контролла
        /// </summary>
        private void row_MouseLeave(object sender, EventArgs e)
        {
            //Меняем цвет заднего плана
            valueInfo.BackColor = infoLabel.BackColor = Color.FromArgb(250, 250, 250);
        }

        /// <summary>
        /// Обработчик события наведения мыши на контролл
        /// </summary>
        private void row_MouseEnter(object sender, EventArgs e)
        {
            //Меняем цвет заднего плана
            valueInfo.BackColor = infoLabel.BackColor = selectColor;
        }
    }
}
