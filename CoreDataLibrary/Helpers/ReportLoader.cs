using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Helpers
{
    public class ReportLoader
    {
        public static List<IReport> LoadReports()
        {
            List<IReport> reportsLoaded = new List<IReport>();

            FileInfo executingAssemblyInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

            string path = executingAssemblyInfo.DirectoryName;
            string reportsPath = executingAssemblyInfo.DirectoryName + @"\Reports\";

            try
            {
                if (Directory.Exists(reportsPath))
                {
                    ICollection<Assembly> assemblies = new List<Assembly>();

                    string[] dllFileNames = Directory.GetFiles(reportsPath, "*.dll");
                    foreach (string dllFile in dllFileNames)
                    {
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        Assembly assembly = Assembly.Load(an);
                        assemblies.Add(assembly);
                    }

                    dllFileNames = Directory.GetFiles(path, "*.dll");
                    foreach (string dllFile in dllFileNames)
                    {
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        try
                        {
                            Assembly assembly = Assembly.Load(an);
                            assemblies.Add(assembly);
                        }
                        catch (FileLoadException)
                        {
                        }
                    }

                    Type reportType = typeof(IReport);
                    ICollection<Type> reportTypes = new List<Type>();
                    foreach (Assembly assembly in assemblies)
                    {
                        if (assembly != null)
                        {
                            Type[] types = assembly.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.IsInterface || type.IsAbstract)
                                {
                                    continue;
                                }
                                if (type.GetInterface(reportType.FullName) != null)
                                {
                                    reportTypes.Add(type);
                                }
                            }
                        }
                    }

                    foreach (Type type in reportTypes)
                    {
                        IReport report = (IReport)Activator.CreateInstance(type);
                        reportsLoaded.Add(report);
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "NoCoreDataReportService", "Report Loader", e);
            }
            return reportsLoaded;
        }

        public static List<ICoreProcess> LoadProcesses()
        {
            List<ICoreProcess> processesLoaded = new List<ICoreProcess>();

            FileInfo executingAssemblyInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

            string path = executingAssemblyInfo.DirectoryName;
            string reportsPath = executingAssemblyInfo.DirectoryName + @"\Reports\";

            try
            {
                if (Directory.Exists(reportsPath))
                {
                    ICollection<Assembly> assemblies = new List<Assembly>();

                    string[] dllFileNames = Directory.GetFiles(reportsPath, "*.dll");
                    foreach (string dllFile in dllFileNames)
                    {
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        Assembly assembly = Assembly.Load(an);
                        assemblies.Add(assembly);
                    }

                    dllFileNames = Directory.GetFiles(path, "*.dll");
                    foreach (string dllFile in dllFileNames)
                    {
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        try
                        {
                            Assembly assembly = Assembly.Load(an);
                            assemblies.Add(assembly);
                        }
                        catch (FileLoadException)
                        {
                        }
                    }

                    Type reportType = typeof(ICoreProcess);
                    ICollection<Type> reportTypes = new List<Type>();
                    foreach (Assembly assembly in assemblies)
                    {
                        if (assembly != null)
                        {
                            Type[] types = assembly.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.IsInterface || type.IsAbstract)
                                {
                                    continue;
                                }
                                if (type.GetInterface(reportType.FullName) != null)
                                {
                                    reportTypes.Add(type);
                                }
                            }
                        }
                    }

                    foreach (Type type in reportTypes)
                    {
                        ICoreProcess process = (ICoreProcess)Activator.CreateInstance(type);
                        processesLoaded.Add(process);
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "NoCoreDataReportService", "Process Loader", e);
            }
            return processesLoaded;
        }

        public static List<ICoreDataWorkItem> LoadWorkItems()
        {
            List<ICoreDataWorkItem> coreDataWorkItems = new List<ICoreDataWorkItem>();

            FileInfo executingAssemblyInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

            string path = executingAssemblyInfo.DirectoryName;
            string reportsPath = executingAssemblyInfo.DirectoryName + @"\WorkItems\";

            try
            {
                if (Directory.Exists(reportsPath))
                {
                    ICollection<Assembly> assemblies = new List<Assembly>();

                    string[] dllFileNames = Directory.GetFiles(reportsPath, "*.dll");
                    foreach (string dllFile in dllFileNames)
                    {
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        Assembly assembly = Assembly.Load(an);
                        assemblies.Add(assembly);
                    }

                    dllFileNames = Directory.GetFiles(path, "*.dll");
                    foreach (string dllFile in dllFileNames)
                    {
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        try
                        {
                            Assembly assembly = Assembly.Load(an);
                            assemblies.Add(assembly);
                        }
                        catch (FileLoadException)
                        {
                        }
                    }

                    Type reportType = typeof(ICoreDataWorkItem);
                    ICollection<Type> reportTypes = new List<Type>();
                    foreach (Assembly assembly in assemblies)
                    {
                        if (assembly != null)
                        {
                            Type[] types = assembly.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.IsInterface || type.IsAbstract)
                                {
                                    continue;
                                }
                                if (type.GetInterface(reportType.FullName) != null)
                                {
                                    reportTypes.Add(type);
                                }
                            }
                        }
                    }

                    foreach (Type type in reportTypes)
                    {
                        ICoreDataWorkItem process = (ICoreDataWorkItem)Activator.CreateInstance(type);
                        coreDataWorkItems.Add(process);
                    }
                }
            }
            catch (Exception e)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "NoCoreDataWorkItems", "WorkItem Loader", e);
            }
            return coreDataWorkItems;
        }
    }
}
