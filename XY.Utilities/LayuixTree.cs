using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace XY.Utilities
{
    public class LayuixTree
    {
        #region treeSelect使用

        public static IList<TreeObject> CreateTree(List<TreeObject> parentTreeObject, List<TreeObject> childrensTreeObject)
        {
            List<TreeObject> nodes = parentTreeObject;
            foreach (TreeObject item in nodes)
            {
                item.children = GetChildrens(item, childrensTreeObject);
            }
            return nodes;
        }
        //递归获取子节点
        public static IList<TreeObject> GetChildrens(TreeObject node, List<TreeObject> childrensTreeObject)
        {
            IList<TreeObject> childrens = childrensTreeObject.Where(it=> node.id == it.pId).Select(x =>new TreeObject { id = x.id, name = x.name,pId = x.pId }).ToList();
            foreach (TreeObject item in childrens)
            {
                item.children = GetChildrens(item, childrensTreeObject);
            }
            return childrens;
        }
        #endregion

        #region LayuixTree使用
        public static IList<XTreeObject> CreateXTree(List<XTreeObject> parentTreeObject, List<XTreeObject> childrensTreeObject)
        {
            List<XTreeObject> nodes = parentTreeObject;
            foreach (XTreeObject item in nodes)
            {
                item.data = GetDatas(item, childrensTreeObject);
            }
            return nodes;
        }
        //递归获取子节点
        public static IList<XTreeObject> GetDatas(XTreeObject node, List<XTreeObject> childrensTreeObject)
        {
            IList<XTreeObject> childrens = childrensTreeObject.Where(it => node.value == it.pId).Select(x => new XTreeObject { value = x.value, title = x.title, pId = x.pId, @checked = x.@checked }).ToList();
            foreach (XTreeObject item in childrens)
            {
                item.data = GetDatas(item, childrensTreeObject);
            }
            return childrens;
        }
        /// <summary>
        /// 三级tree树绑定值
        /// </summary>
        /// <param name="parentTreeObject">一级</param>
        /// <param name="childrensTreeObject">二级</param>
        /// <param name="threelevelTreeObject">三级</param>
        /// <returns></returns>
        public static IList<XTreeObject> CreateXTree(List<XTreeObject> parentTreeObject, List<XTreeObject> childrensTreeObject, List<XTreeObject> threelevelTreeObject)
        {
            List<XTreeObject> nodes = parentTreeObject;
            foreach (XTreeObject item in nodes)
            {
                item.data = GetDatas(item, childrensTreeObject, threelevelTreeObject);
            }
            return nodes;
        }
        //二级递归获取子节点
        public static IList<XTreeObject> GetDatas(XTreeObject node, List<XTreeObject> childrensTreeObject, List<XTreeObject> threelevelTreeObject)
        {
            IList<XTreeObject> childrens = childrensTreeObject.Where(it => node.value == it.pId).Select(x => new XTreeObject { value = x.value, title = x.title, pId = x.pId, @checked = x.@checked }).ToList();
            foreach (XTreeObject item in childrens)
            {
                foreach (XTreeObject chilitem in childrens)
                {
                    chilitem.data = threelevelTreeObject.Where(it => chilitem.value == it.pId).Select(x => new XTreeObject { value = x.value, title = x.title, pId = x.pId, @checked = x.@checked }).ToList(); ;
                }
                item.data = GetDatas(item, childrensTreeObject, threelevelTreeObject);
            }
            return childrens;
        }
       

        #endregion
    }
    /// <summary>
    /// treeSelect对象
    /// </summary>
    public class TreeObject
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
        public IList<TreeObject> children = new List<TreeObject>();
        public virtual void Addchildren(TreeObject node)
        {
            this.children.Add(node);
        }
    }
    /// <summary>
    /// LayuixTree对象
    /// </summary>
    public class XTreeObject
    {
        public string value { get; set; }
        public string pId { get; set; }
        public string title { get; set; }
        public bool disabled { get; set; }
        public bool @checked { get; set; }
        public IList<XTreeObject> data = new List<XTreeObject>();
        public virtual void Addchildren(XTreeObject node)
        {
            this.data.Add(node);
        }
    }

    /// <summary>
    /// TreeTable对象
    /// </summary>
    public class TreeTable
    {
        public string id { get; set; }     
        public string pid { get; set; }
        public string title { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 英文代码
        /// </summary>
        public string EnCode { get; set; }
        /// <summary>
        /// 机构管理人
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }
        /// <summary>
        /// 机构类型
        /// </summary>
        public string OrgType { get; set; }

    }


}
