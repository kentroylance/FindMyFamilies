﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FindMyFamiles.Services.Util {
    public class UtilityTest {
        public void FindConflictingReferences() {
            List<Assembly> assemblies = GetAllAssemblies(@"F:\dev\source\FindMyFamilies\Web\bin");

            List<Reference> references = GetReferencesFromAllAssemblies(assemblies);

            IEnumerable<IGrouping<string, Reference>> groupsOfConflicts = FindReferencesWithTheSameShortNameButDiffererntFullNames(references);

            foreach (var group in groupsOfConflicts) {
                Console.Out.WriteLine("Possible conflicts for {0}:", group.Key);
                foreach (Reference reference in group) {
                    Console.Out.WriteLine("{0} references {1}", reference.Assembly.Name.PadRight(25), reference.ReferencedAssembly.FullName);
                }
            }
        }

        private IEnumerable<IGrouping<string, Reference>> FindReferencesWithTheSameShortNameButDiffererntFullNames(List<Reference> references) {
            return from reference in references group reference by reference.ReferencedAssembly.Name
                into referenceGroup where referenceGroup.ToList().Select(reference => reference.ReferencedAssembly.FullName).Distinct().Count() > 1 select referenceGroup;
        }

        private List<Reference> GetReferencesFromAllAssemblies(List<Assembly> assemblies) {
            var references = new List<Reference>();
            foreach (Assembly assembly in assemblies) {
                foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies()) {
                    references.Add(new Reference {Assembly = assembly.GetName(), ReferencedAssembly = referencedAssembly});
                }
            }
            return references;
        }

        private List<Assembly> GetAllAssemblies(string path) {
            var files = new List<FileInfo>();
            var directoryToSearch = new DirectoryInfo(path);
            files.AddRange(directoryToSearch.GetFiles("*.dll", SearchOption.AllDirectories));
            files.AddRange(directoryToSearch.GetFiles("*.exe", SearchOption.AllDirectories));
            return files.ConvertAll(file => Assembly.LoadFile(file.FullName));
        }

        private class Reference {
            public AssemblyName Assembly {
                get;
                set;
            }

            public AssemblyName ReferencedAssembly {
                get;
                set;
            }
        }
    }
}