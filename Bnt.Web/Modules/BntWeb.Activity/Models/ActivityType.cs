/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		Activitys
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/17 10:00:04
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Activity.Models
{
    [Table(KeyGenerator.TablePrefix + "Activity_Type")]
    public class ActivityType
    {
        public Guid Id { set; get; }

        public string TypeName { set; get; }

        public string Description { set; get; }
    }
}