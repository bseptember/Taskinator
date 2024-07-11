using Realms;
using Realms.Schema;

namespace Taskinator.Helper
{
    public class RealmMigrationHelper
    {
        public void OnMigration(Migration migration, ulong oldSchemaVersion)
        {
            var oldRealm = migration.OldRealm;
            var newRealm = migration.NewRealm;

            // Handle type removals
            foreach (var oldSchema in oldRealm.Schema)
            {
                if (!newRealm.Schema.Any(s => s.Name == oldSchema.Name))
                {
                    migration.RemoveType(oldSchema.Name);
                }
            }

            // Handle property renaming and additions
            foreach (var newSchema in newRealm.Schema)
            {
                var oldSchema = oldRealm.Schema.FirstOrDefault(s => s.Name == newSchema.Name);
               
                if (oldSchema == null)
                    throw new Exception("Old schema not valid");

                if (!oldSchema.Equals(default(ObjectSchema)))
                {
                    foreach (var newProp in newSchema)
                    {
                        var oldProp = oldSchema.FirstOrDefault(p => p.Name == newProp.Name);
                        if (oldProp.Equals(default(Property)))
                        {
                            // Property is new, handle any necessary initialization here
                            // Example: Initialize with default values
                        }
                    }

                    foreach (var oldProp in oldSchema)
                    {
                        var newProp = newSchema.FirstOrDefault(p => p.Name == oldProp.Name);
                        if (newProp.Equals(default(Property)))
                        {
                            // Property is renamed, handle renaming
                            var newPropertyName = DetermineNewPropertyName(oldProp.Name);
                            if (!string.IsNullOrEmpty(newPropertyName))
                            {
                                migration.RenameProperty(newSchema.Name, oldProp.Name, newPropertyName);
                            }
                        }
                    }
                }

            }
        }

        private string DetermineNewPropertyName(string oldPropertyName)
        {
            return ConvertToPascalCase(oldPropertyName);
        }

        private string ConvertToPascalCase(string oldPropertyName)
        {
            if (string.IsNullOrEmpty(oldPropertyName))
            {
                return string.Empty;
            }

            // Convert first character to uppercase and the rest remain unchanged
            return char.ToUpper(oldPropertyName[0]) + oldPropertyName.Substring(1);
        }
    }
}
