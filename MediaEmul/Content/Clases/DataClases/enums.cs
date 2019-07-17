using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEmul.Content.Clases.DataClases
{
    /// <summary>
    /// Класс, содержащий все перечисления проекта
    /// </summary>
    class enums
    {
        /// <summary>
        /// Перечисление типов данных взятых из буфера обмена
        /// </summary>
        public enum clipboardDataType
        {
            Текст, 
            Файлы,
            Картинка,
            Аудиопоток
        }
    }
}
