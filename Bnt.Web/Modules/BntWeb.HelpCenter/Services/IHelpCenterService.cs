/* 
    ======================================================================== 
        File name：        IHelpCenterService
        Module:                
        Author：            Kahr.Lu(陆康康)
        Create Time：    2016/6/30 14:42:52
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BntWeb.HelpCenter.Models;

namespace BntWeb.HelpCenter.Services
{
    public interface IHelpCenterService : IDependency
    {
        #region 帮助
        /// <summary>
        /// 帮助分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Help> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Help, bool>> expression,
            Expression<Func<Help, TKey>> orderByExpression, bool isDesc, out int totalCount);

        /// <summary>
        /// 根据Id查询帮助
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Help GetHelpById(Guid id);

        /// <summary>
        /// 根据Id查询帮助及分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Help GetOneHelpById(Guid id);

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="help"></param>
        /// <returns></returns>
        bool Delete(Help help);

        /// <summary>
        /// 创建帮助
        /// </summary>
        /// <param name="help"></param>
        /// <returns></returns>
        bool CreateHelp(Help help);

        /// <summary>
        /// 更新帮助
        /// </summary>
        /// <param name="help"></param>
        /// <returns></returns>
        bool UpdateHelp(Help help);

        /// <summary>
        /// 根据类别查询帮助分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Help> GetListPagedByCategory<TKey>(Guid categoryId, int pageIndex, int pageSize,
            out int totalCount);

        /// <summary>
        /// 帮助搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Help> Search(string keyword, int pageIndex, int pageSize, out int totalCount);

        #endregion

        #region 帮助类别

        /// <summary>
        /// 获取帮助类别列表
        /// </summary>
        /// <returns></returns>
        List<HelpCategory> GetCategories();
        /// <summary>
        /// 根据Id获取帮助类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HelpCategory GetCategoryById(Guid id);
        /// <summary>
        /// 根据类别名获取帮助类别
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        HelpCategory GetCategoryByName(string name);
        /// <summary>
        /// 保存帮助类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool SaveCategory(HelpCategory model);
        /// <summary>
        /// 删除帮助类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool DeleteCategory(HelpCategory model);

        #endregion

    }
}