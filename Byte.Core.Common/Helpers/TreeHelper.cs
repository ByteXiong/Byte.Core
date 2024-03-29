using Byte.Core.Common.Extensions;
using Byte.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Common.Helpers
{
    /// <summary>
    /// 树结构帮助类
    /// </summary>
    public class TreeHelper
    {
        #region 外部接口

        /// <summary>
        /// 建造树结构
        /// </summary>
        /// <param name="allNodes">所有的节点</param>
        /// <param name="parentId">父节点</param>
        /// <returns></returns>
        public static List<T> GetTree<T, TKey>(List<T> allNodes, TKey parentId) where T : TreeModel<TKey>, new()
        {
            List<T> resData = new List<T>();

            var rootNodes = allNodes.Where(x => x.Id.Equals(parentId) ).ToList();
            resData = rootNodes;
            resData.ForEach(aRootNode =>
            {
                if (HaveChildren(allNodes, aRootNode.Id))
                    aRootNode.Children = _GetChildren<T, TKey>(allNodes, aRootNode);
            });

            return resData;
        }

        /// <summary>
        /// 获取所有子节点
        /// 注：包括自己
        /// </summary>
        /// <typeparam name="T">节点类型</typeparam>
        /// <param name="allNodes">所有节点</param>
        /// <param name="parentId">父节点</param>
        /// <param name="isMy">是否包括自己</param>
        /// <returns></returns>
        public static List<T> GetChildren<T, TKey>(List<T> allNodes, TKey parentId, bool? isMy=true) where T : TreeModel<TKey>, new()
        {
            List<T> resList = new List<T>();
            var pmodel = allNodes.FirstOrDefault(x => x.Id.Equals(parentId) );
            if (pmodel == null) return null;
                if (isMy==true)
                resList.Add(pmodel);
           
            _getChildren(allNodes, pmodel, resList);

            return resList;

            void _getChildren(List<T> _allNodes, T pmodel, List<T> _resNodes)
            {
                var children = _allNodes.Where(x => x.ParentId.Equals(pmodel.Id)).ToList();
                _resNodes.AddRange(children);
                children.ForEach(aChild =>
                {
                    _getChildren(_allNodes, aChild, _resNodes);
                });
            }
        }

        /// <summary>
        /// 递归数组扁平化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="allNodes"></param>
        /// <returns></returns>
        public static List<T> GetTreeAry<T,TKey>(List<T> allNodes) where T : TreeModel<TKey>, new()
        {

            List<T> resList = new List<T>();
            allNodes.ForEach(pmodel =>
            {
                resList.Add(pmodel);
                _getChildren(allNodes, pmodel, resList);
            });

            return resList;

            void _getChildren(List<T> _allNodes, T pmodel, List<T> _resNodes)
            {
                var children = _allNodes.Where(x => x.ParentId.Equals(pmodel.Id)).ToList();
                _resNodes.AddRange(children);
                children.ForEach(aChild =>
                {
                    _getChildren(_allNodes, aChild, _resNodes);
                });
            }

            
        }
        


        ///// <summary>
        ///// 递归路由
        ///// </summary>
        ///// <param name="pid">父级Id</param>
        ///// <param name="demos">数据源</param>
        ///// <returns></returns>
        //private List<Guid?> GetParent<T>(Guid? parentId, List<T> units = null) where T : TreeModel, new()
        //{
        //    /*
        //     思路：1.从数据源中找到父级
        //           2.循环父级并赋值，再循环父级时查找子集
        //           3.如果有子集调用用GetMenu（父级Id,数据源）方法一层一层向下找
        //           4.注意：（，是套娃模式。也就是循环第二层第三层的时候还是在一个父
        //           级下面）个人注意点因为我在这里差点没想通，总是想着是平级
        //     */
        //    var parent = units.Where(x => x.ParentId == parentId);
        //    List<UnitChildrenDTO> lists = new List<UnitChildrenDTO>();
        //    foreach (var item in parent)
        //    {
        //        UnitChildrenDTO DemosChilder = new UnitChildrenDTO()
        //        {
        //            Id = item.Id,
        //            Name = item.Name,
        //            IsSelect = item.IsSelect,
        //            Sort = item.Sort,
        //        };
        //        DemosChilder.Children = GetChildren(DemosChilder, units);
        //        lists.Add(DemosChilder);
        //    }

        //    /// <summary>
        //    /// 找子集有就返回NUll，并执行Add
        //    /// </summary>
        //    /// <param name="demos">父级Id</param>
        //    /// <param name="demosd">数据源</param>
        //    /// <returns></returns>
        //    List<UnitChildrenDTO> GetChildren(UnitChildrenDTO childrens, List<Unit> units = null)
        //    {
        //        if (!units.Exists(x => x.ParentId == childrens.Id))
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return GetParent(childrens.Id, units);
        //        }
        //    }
        //    return lists;
        //}
        #endregion

        #region 私有成员

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <typeparam name="T">树模型（TreeModel或继承它的模型）</typeparam>
        /// <param name="nodes">所有节点列表</param>
        /// <param name="parentNode">父节点Id</param>
        /// <returns></returns>
        private static List<object> _GetChildren<T, TKey>(List<T> nodes, T parentNode) where T : TreeModel<TKey>, new()
        {
            Type type = typeof(T);
            var properties = type.GetProperties().ToList();
            List<object> resData = new List<object>();
            var children = nodes.Where(x => x.ParentId.Equals(parentNode.Id)).ToList();
            children.ForEach(aChildren =>
            {
                T newNode = new T();
                resData.Add(newNode);

                //赋值属性
                properties.Where(x => x.CanWrite).ForEach(aProperty =>
                {
                    var value = aProperty.GetValue(aChildren, null);
                    aProperty.SetValue(newNode, value);
                });
                //设置深度
                newNode.Level = parentNode.Level + 1;

                if (HaveChildren(nodes, aChildren.Id))
                    newNode.Children = _GetChildren<T,TKey>(nodes, newNode);
            });

            return resData;
        }

        /// <summary>
        /// 判断当前节点是否有子节点
        /// </summary>
        /// <typeparam name="T">树模型</typeparam>
        /// <param name="nodes">所有节点</param>
        /// <param name="nodeId">当前节点Id</param>
        /// <returns></returns>
        private static bool HaveChildren<T,TKey>(List<T> nodes, TKey nodeId) where T : TreeModel<TKey>, new()
        {
            return nodes.Exists(x => x.ParentId.Equals(nodeId) );
        }

        #endregion
    }
}
