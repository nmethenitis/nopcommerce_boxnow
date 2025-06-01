using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Shipping.BoxNow.Domain;

namespace Nop.Plugin.Shipping.BoxNow.Data;
public class BoxNowRecordBuilder : NopEntityBuilder<BoxNowRecord> {
    public override void MapEntity(CreateTableExpressionBuilder table) {
        table
           .WithColumn(nameof(BoxNowRecord.OrderId))
           .AsInt32()
           .WithColumn(nameof(BoxNowRecord.LockerId))
           .AsInt32()
           .WithColumn(nameof(BoxNowRecord.ParcelId))
           .AsString(500)
           .Nullable();
    }
}
