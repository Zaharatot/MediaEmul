using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEmul.Content.Clases.DataClases
{
    /// <summary>
    /// Класс, хранящий события
    /// </summary>
    public class events
    {
        /// <summary>
        /// Делегат события обновления буфера
        /// </summary>
        /// <param name="text">Текст буфера</param>
        /// <param name="id">Id буфера</param>
        public delegate void updateBuffer(string text, int id);

        /// <summary>
        /// Делегат события запроса отображения 
        /// формы со списком буферов
        /// </summary>
        public delegate void showbufferForm();
    }
}
