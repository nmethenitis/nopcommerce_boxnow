using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Shipping.BoxNow.Domain;

namespace Nop.Plugin.Shipping.BoxNow.Data;
public class BoxNowRecordBuilder : NopEntityBuilder<BoxNowRecord> {
    public override void MapEntity(CreateTableExpressionBuilder table) {
        table
           .WithColumn(nameof(BoxNowRecord.OrderGuid))
           .AsGuid()
           .WithColumn(nameof(BoxNowRecord.LockerId))
           .AsInt32()
           .WithColumn(nameof(BoxNowRecord.LabelUrl))
           .AsString(500)
           .Nullable();
    }
}
