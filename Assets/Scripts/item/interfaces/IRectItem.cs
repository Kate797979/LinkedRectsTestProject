using Assets.Scripts.item.interfaces;
using UnityEngine;

namespace Assets.Scripts.item
{
    public interface IRectItem
    {
        GameObject gameObject { get; }

        RectTransform rectTransform { get; }

        /// <summary>
        /// Пытается найти связь с указанным объектом
        /// </summary>
        /// <param name="toItem">С каким объектом ищется связь</param>
        /// <param name="itemsLink">Если связь существует, то возвращает объект связи</param>
        /// <returns>Возвращает true, если существует связь с указанным объектом</returns>
        bool TryGetLinkToItem(IRectItem toItem, out IRectItemsLink itemsLink);

        /// <summary>
        /// Добавляет связь с указанным объектом
        /// </summary>
        /// <param name="toItem"></param>
        /// <param name="link"></param>
        void AddLink(IRectItem toItem, IRectItemsLink link);

        /// <summary>
        /// Удаляет связь с указанным объектом
        /// </summary>
        /// <param name="toItem"></param>
        void RemoveLink(IRectItem toItem);

        /// <summary>
        /// 
        /// </summary>
        void Destroy();

    }
}
