using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Shipping.BoxNow.Domain;
public class BoxNowRecord : BaseEntity {
    public Guid Id { get; set; }
    public Guid OrderGuid { get; set; }
    public int LockerId { get; set; }
    public string LabelUrl { get; set; }
}
