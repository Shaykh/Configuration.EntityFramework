﻿using System;
using System.Collections.ObjectModel;

namespace Configuration.EntityFramework.Samples
{
    public class Database
    {
        public Database Create()
        {
            using (var context = new ConfigurationContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            return this;
        }

        public Database Seed()
        {
            this.SeedAppSetting();
            this.SeedSectionWithChild();
            this.SeedSectionWithChildren();
            return this;
        }

        public Database SeedAppSetting()
        {
            using (var context = new ConfigurationContext())
            {
                var section = new SectionEntity() { ApplicationName = "Sample", SectionName = "appSettings" };
                context.Sections.Add(section);
                context.SaveChanges();

                var setting = new SettingEntity() { SectionId = section.Id, Key = "TestSetting", Json = @"""Test Value""" };
                context.Settings.Add(setting);
                context.SaveChanges();
            }
            return this;
        }

        public Database SeedSectionWithChild()
        {
            using (var context = new ConfigurationContext())
            {
                var complex = new SectionWithChild()
                {
                    Id = Guid.NewGuid(),
                    Name = "parent",
                    Child = new Child()
                    {
                        Id = Guid.NewGuid(),
                        Name = "child"
                    }
                };

                var section = new SectionEntity() { ApplicationName = "Sample", SectionName = "SectionWithChild" };
                context.Sections.Add(section);
                context.SaveChanges();

                var setting = new SettingEntity() { SectionId = section.Id, Key = "SectionWithChild" };
                setting.SetValue(complex);

                context.Settings.Add(setting);
                context.SaveChanges();
            }
            return this;
        }

        public Database SeedSectionWithChildren()
        {
            using (var context = new ConfigurationContext())
            {
                var complex = new SectionWithChildren()
                {
                    Id = Guid.NewGuid(),
                    Name = "parent",
                };
                complex.Children = new Collection<Child>();
                complex.Children.Add
                (
                    new Child()
                    {
                        Id = Guid.NewGuid(),
                        Name = "child1"
                    }
                );
                complex.Children.Add
                (
                    new Child()
                    {
                        Id = Guid.NewGuid(),
                        Name = "child2"
                    }
                );

                var section = new SectionEntity() { ApplicationName = "Sample", SectionName = "SectionWithChildren" };
                context.Sections.Add(section);
                context.SaveChanges();

                var setting = new SettingEntity() { SectionId = section.Id, Key = "SectionWithChildren" };
                setting.SetValue(complex);

                context.Settings.Add(setting);
                context.SaveChanges();
            }
            return this;
        }
    }
}
