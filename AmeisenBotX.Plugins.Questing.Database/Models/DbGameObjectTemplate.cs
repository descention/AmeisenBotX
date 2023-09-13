using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Models
{
    [Keyless]
    [Table("gameobject_template")]
    public record DbGameObjectTemplate
    {
        public string AIName { get; set; }
        public string name { get; set; }
        public string IconName { get; set; }
        public string castBarCaption { get; set; }
        public string unk1 { get; set; }
        public string ScriptName { get; set; }
        public int flags { get; set; }
        public int questItem1 { get; set; }
        public int questItem2 { get; set; }
        public int questItem3 { get; set; }
        public int questItem4 { get; set; }
        public int questItem5 { get; set; }
        public int questItem6 { get; set; }
        public int data0 { get; set; }
        public int data1 { get; set; }
        public int data2 { get; set; }
        public int data3 { get; set; }
        public int data4 { get; set; }
        public int data5 { get; set; }
        public int data6 { get; set; }
        public int data7 { get; set; }
        public int data8 { get; set; }
        public int data9 { get; set; }
        public int data10 { get; set; }
        public int data11 { get; set; }
        public int data12 { get; set; }
        public int data13 { get; set; }
        public int data14 { get; set; }
        public int data15 { get; set; }
        public int data16 { get; set; }
        public int data17 { get; set; }
        public int data18 { get; set; }
        public int data19 { get; set; }
        public int data20 { get; set; }
        public int data21 { get; set; }
        public int data22 { get; set; }
        public int data23 { get; set; }
        public int data24 { get; set; }
        public int data25 { get; set; }
        public int data26 { get; set; }
        public int data27 { get; set; }
        public int data28 { get; set; }
        public int data29 { get; set; }
        public int data30 { get; set; }
        public int data31 { get; set; }
        public int unkInt32 { get; set; }
        public float size { get; set; }
        public int entry { get; set; }
        public int displayId { get; set; }
        public short faction { get; set; }
        public short WDBVerified { get; set; }
        public short type { get; set; }
    }
}
