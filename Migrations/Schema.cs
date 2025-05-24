using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Shipping.BoxNow.Domain;

namespace Nop.Plugin.Shipping.BoxNow.Migrations;
[NopMigration("2025/05/11 19:27:00", "Shipping.BoxNow base schema", MigrationProcessType.Installation)]
public class Schema : AutoReversingMigration {
    public override void Up() {
        Create.TableFor<BoxNowRecord>();
    }
}
