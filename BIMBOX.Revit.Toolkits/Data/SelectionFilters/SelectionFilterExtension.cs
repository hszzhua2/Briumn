﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension.Data.SelectionFilters
{
    public static class SelectionFilterExtension
    {
        /// <summary>
        /// this class is used to filter elements where class is the same as imput class
        /// this class is also used to filter where is loacted in assign level's elements
        /// </summary>
        /// <typeparam name="TElement">element target class</typeparam>
        public class ElementSelectionFilter<TElement> : ISelectionFilter where TElement : Element
        {
            private IEnumerable<ElementId> _levelIds = null;
            private readonly Func<TElement, bool> _filterCodition;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="levelIds">指定楼层的Id</param>
            /// <param name="filterCodition">自定义过滤条件</param>
            public ElementSelectionFilter(IEnumerable<ElementId> levelIds = null, Func<Element, bool> filterCodition = null)
            {
                _levelIds = levelIds;
                _filterCodition = filterCodition;
            }

            public bool AllowElement(Element element)
            {
                if (element is TElement)
                {
                    TElement result = element is TElement tElement ? tElement : null;
                    if (_levelIds != null)
                    {
                        if (element.LevelId != ElementId.InvalidElementId && _levelIds.FirstOrDefault(x => x == element.LevelId) == null)
                        {
                            result = null;
                        }
                    }
                    if (_filterCodition != null && !_filterCodition.Invoke(result))
                    {
                        result = null;
                    }
                    if (result != null)
                    {
                        return true;
                    }
                    return false;
                }
                return false;

            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// this class is used to filter geometry object class is the same as imput class
        /// this class is also used to filter geometry object where is belong to assign element class
        /// 这个类用于指定类型的几何图形，且可指定目标构件
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TGeometryObject"></typeparam>
        public class GeometrySelectionFilter<TGeometryObject> : ISelectionFilter where TGeometryObject : GeometryObject
        {
            private readonly Document _document;
            private readonly BuiltInCategory _builtInCategory = BuiltInCategory.INVALID;
            private readonly IEnumerable<ElementId> _elementIds;
            private readonly Func<TGeometryObject, bool> _filterCodition;

            /// <summary>
            /// 构造函数，当不需要指定构件类型时使用
            /// </summary>
            /// <param name="document">当前文档</param>
            /// <param name="elementIds">限定构件</param>
            /// <param name="filterCodition">自定义条件限制选取的几何形状</param>
            public GeometrySelectionFilter(Document document, IEnumerable<ElementId> elementIds = null, Func<TGeometryObject, bool> filterCodition = null)
            {
                _document = document;
                _elementIds = elementIds;
                _filterCodition = filterCodition;
            }

            /// <summary>
            /// 构造函数，需要指定构件类型时使用
            /// </summary>
            /// <param name="document"></param>
            /// <param name="builtInCategory">指定构件类别</param>
            /// <param name="elementIds"></param>
            /// <param name="filterCodition"></param>
            public GeometrySelectionFilter(Document document, BuiltInCategory builtInCategory, IEnumerable<ElementId> elementIds = null, Func<GeometryObject, bool> filterCodition = null)
            {
                _document = document;
                _builtInCategory = builtInCategory;
                _elementIds = elementIds;
                _filterCodition = filterCodition;
            }

            public bool AllowElement(Element element)
            {
                var result = element;
                if (_builtInCategory != BuiltInCategory.INVALID && element.Category != null)
                {
                    if (element.Category.Id.IntegerValue != (int)_builtInCategory)
                    {
                        result = null;
                    }
                }
                if (_elementIds != null && _elementIds.FirstOrDefault(x => x == element.Id) == null)
                {
                    result = null;
                }
                if (result == element)
                {
                    return true;
                }
                return false;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                if (_document.GetElement(reference).GetGeometryObjectFromReference(reference) is TGeometryObject)
                {
                    if (_filterCodition != null)
                    {
                        var geometryObject = _document.GetElement(reference).GetGeometryObjectFromReference(reference);
                        if (geometryObject != null && geometryObject is TGeometryObject item)
                        {
                            return _filterCodition.Invoke(item);
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// this class is used to filter element in link document object class is the same as imput class 
        /// 这个类用于选择时过滤指定链接文件中的指定类型的构件,使用时
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        public class RevitLinkElementFilter<TElement> : ISelectionFilter where TElement : Element
        {
            private readonly IEnumerable<RevitLinkInstance> _linkInstances;
            private readonly Func<TElement, bool> _filterCodition;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="linkInstances">指定过滤的链接模型</param>
            /// <param name="filterCodition">自定义目标类别构件的过滤条件</param>
            public RevitLinkElementFilter(IEnumerable<RevitLinkInstance> linkInstances = null, Func<TElement, bool> filterCodition = null)
            {
                _linkInstances = linkInstances;
                _filterCodition = filterCodition;
            }


            public bool AllowElement(Element elem)
            {
                if (_linkInstances == null)
                {
                    return true;
                }
                else
                {
                    var resultLinkInstance = _linkInstances.Where(x => x.Id == elem.Id);
                    if (resultLinkInstance != null && resultLinkInstance.Any())
                    {
                        return true;
                    }
                    return false;
                }
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                foreach (var linkInstance in _linkInstances)
                {
                    try
                    {
                        var element = linkInstance.GetLinkDocument()?.GetElement(reference.LinkedElementId) is TElement tElement ? tElement : null;
                        if (element == null && element.IsValidObject != true)
                        {
                            element = null;
                        }
                        if (_filterCodition != null && !_filterCodition.Invoke(element))
                        {
                            element = null;
                        }
                        if (element != null)
                        { return true; }
                        return false;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return false;
            }
        }
    }
}
