﻿using PipeIPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrivateWin10
{
    public class Priv10Client: PipeClient
    {
        public Priv10Client(string name)
        {
            Name = name;
        }

        public FirewallManager.FilteringModes GetFilteringMode()
        {
            return RemoteExec("GetFilteringMode", null, FirewallManager.FilteringModes.Unknown);
        }

        public bool SetFilteringMode(FirewallManager.FilteringModes Mode)
        {
            return RemoteExec("SetFilteringMode", Mode, false);
        }

        public bool IsFirewallGuard()
        {
            return RemoteExec("IsFirewallGuard", null, false);
        }

        public bool SetFirewallGuard(bool guard, FirewallGuard.Mode mode)
        {
            return RemoteExec("SetFirewallGuard", new object[2] { guard, mode }, false);
        }

        public FirewallMonitor.Auditing GetAuditPolicy()
        {
            return RemoteExec("GetAuditPolicy", null, FirewallMonitor.Auditing.Off);
        }

        public bool SetAuditPolicy(FirewallMonitor.Auditing audit)
        {
            return RemoteExec("SetAuditPolicy", audit, false);
        }

        public List<ProgramSet> GetPrograms(List<Guid> guids = null)
        {
            return RemoteExec<List<ProgramSet>>("GetPrograms", new object[1] { guids }, null);
        }

        public ProgramSet GetProgram(ProgramID id, bool canAdd = false)
        {
            return RemoteExec<ProgramSet>("GetProgram", new object[2] { id, canAdd }, null);
        }
        
        public bool AddProgram(ProgramID id, Guid guid)
        {
            return RemoteExec("AddProgram", new object[2] { id, guid }, false);
        }
        
        public bool UpdateProgram(Guid guid, ProgramSet.Config config, UInt64 expiration = 0)
        {
            return RemoteExec("UpdateProgram", new object[3] { guid, config, expiration }, false);
        }

        public bool MergePrograms(Guid to, Guid from)
        {
            return RemoteExec("MergePrograms", new object[2] { to, from }, false);
        }

        public bool SplitPrograms(Guid from, ProgramID id)
        {
            return RemoteExec("SplitPrograms", new object[2] { from, id }, false);
        }

        public bool RemoveProgram(Guid guid, ProgramID id = null)
        {
            return RemoteExec("RemoveProgram", new object[2] { guid, id }, false);
        }
        
        public bool LoadRules()
        {
            return RemoteExec("LoadRules", null, false);
        }

        public Dictionary<Guid, List<FirewallRuleEx>> GetRules(List<Guid> guids = null)
        {
            return RemoteExec<Dictionary<Guid, List<FirewallRuleEx>>>("GetRules", guids, null);
        }

        public bool UpdateRule(FirewallRule rule, UInt64 expiration = 0)
        {
            return RemoteExec("UpdateRule", new object[2] { rule, expiration }, false);
        }
        
        public bool RemoveRule(FirewallRule rule)
        {
            return RemoteExec("RemoveRule", rule, false);
        }

        public int SetRuleApproval(Engine.ApprovalMode Mode, FirewallRule rule)
        {
            return RemoteExec("SetRuleApproval", new object[2] { Mode, rule }, 0);
        }

        public bool BlockInternet(bool bBlock)
        {
            return RemoteExec("BlockInternet", bBlock, false);
        }
        
        public bool ClearLog(bool ClearSecLog)
        {
            return RemoteExec("ClearLog", ClearSecLog, false);
        }
        
        public int CleanUpPrograms()
        {
            return RemoteExec("CleanUpPrograms", null, 0);
        }

        public int CleanUpRules()
        {
            return RemoteExec("CleanUpRules", null, 0);
        }
        
        public Dictionary<Guid, List<Program.LogEntry>> GetConnections(List<Guid> guids = null)
        {
            return RemoteExec<Dictionary<Guid, List<Program.LogEntry>>>("GetConnections", guids, null);
        }

        public Dictionary<Guid, List<NetworkSocket>> GetSockets(List<Guid> guids = null)
        {
            return RemoteExec<Dictionary<Guid, List<NetworkSocket>>>("GetSockets", guids, null);
        }
        public Dictionary<Guid, List<Program.DnsEntry>> GetDomains(List<Guid> guids = null)
        {
            return RemoteExec<Dictionary<Guid, List<Program.DnsEntry>>>("GetDomains", guids, null);
        }


        public bool ApplyTweak(TweakManager.Tweak tweak)
        {
            return RemoteExec("ApplyTweak", tweak, false);
        }

        public bool TestTweak(TweakManager.Tweak tweak)
        {
            return RemoteExec("TestTweak", tweak, false);
        }

        public bool UndoTweak(TweakManager.Tweak tweak)
        {
            return RemoteExec("UndoTweak", tweak, false);
        }


        public bool Quit()
        {
            return RemoteExec("Quit", null, false);
        }


        public event EventHandler<Engine.FwEventArgs> ActivityNotification;
        public event EventHandler<Engine.ChangeArgs> ChangeNotification;
        

        public override void HandlePushNotification(string func, object args)
        {
            try
            {
                if (Application.Current == null)
                    return; // not ready yet

                if (func == "ActivityNotification")
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                        ActivityNotification?.Invoke(this, (Engine.FwEventArgs)args);
                    }));
                }
                else if (func == "ChangeNotification")
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                        ChangeNotification?.Invoke(this, (Engine.ChangeArgs)args);
                    }));   
                }
                else
                {
                    throw new Exception("Unknown Notificacion");
                }
            }
            catch (Exception err)
            {
                AppLog.Exception(err);
            }
        }
       
    }
}
