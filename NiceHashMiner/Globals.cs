using System;
using System.Collections.Generic;
using NiceHashMiner.Enums;
using Newtonsoft.Json;

namespace NiceHashMiner {
    public class Globals {
        // Constants
        public static string[] MiningLocation = { "eu", "usa", "hk", "jp", "in", "br" };
        public static readonly string DemoUser = "1ErgjhC2MunmYbBVLk14Wr72CyTRMxCJS3";
        public static readonly string MPHDemoUser = "pb7280";
        // change this if TOS changes
        public static int CURRENT_TOS_VER = 3;

        // Variables
        public static Dictionary<AlgorithmType, NiceHashSMA> NiceHashData = null;
        public static Dictionary<AlgorithmType, NiceHashSMA> MPHData = null;
        public static double BitcoinUSDRate;
        public static JsonSerializerSettings JsonSettings = null;
        public static int ThreadsPerCPU;
        public static string BTCAddress = DemoUser;
        public static string MPHAddress = MPHDemoUser;
        // quickfix guard for checking internet conection
        public static bool IsFirstNetworkCheckTimeout = true;
        public static int FirstNetworkCheckTimeoutTimeMS = 500;
        public static int FirstNetworkCheckTimeoutTries = 10;
        

        public static string GetLocationURL(AlgorithmType AlgorithmType, string miningLocation, NHMConectionType ConectionType) {
            if (Globals.NiceHashData != null && Globals.NiceHashData.ContainsKey(AlgorithmType)) {
                double paying = Globals.NiceHashData[AlgorithmType].paying;
                string prefix = "";
                if (Globals.MPHData != null && Globals.MPHData.ContainsKey(AlgorithmType)) {
                    if (Globals.MPHData[AlgorithmType].paying > paying) {
                        paying = Globals.MPHData[AlgorithmType].paying;

                        int mphport = Globals.MPHData[AlgorithmType].port;
                        string host = Globals.MPHData[AlgorithmType].host;
                        if (NHMConectionType.STRATUM_TCP == ConectionType || NHMConectionType.STRATUM_SSL == ConectionType) {
                            prefix = "stratum+tcp://";
                        }
                        return prefix + host + ":" + mphport;
                    }
                }
                string name = Globals.NiceHashData[AlgorithmType].name;
                int n_port = Globals.NiceHashData[AlgorithmType].port;
                int ssl_port = 30000 + n_port;

                // NHMConectionType.NONE
                int port = n_port;
                if (NHMConectionType.LOCKED == ConectionType) {
                    return miningLocation;
                }
                if (NHMConectionType.STRATUM_TCP == ConectionType) {
                    prefix = "stratum+tcp://";
                }
                if (NHMConectionType.STRATUM_SSL == ConectionType) {
                    prefix = "stratum+ssl://";
                    port = ssl_port;
                }

                return prefix
                        + name
                        + "." + miningLocation
                        + ".nicehash.com:"
                        + port;
                
            }
            return "";
        }
        public static string GetMostProfitableAddress(AlgorithmType AlgorithmType) {
            double paying = 0;
            if (Globals.NiceHashData != null && Globals.NiceHashData.ContainsKey(AlgorithmType)) {
                paying = Globals.NiceHashData[AlgorithmType].paying;
            }
            if (Globals.MPHData != null && Globals.MPHData.ContainsKey(AlgorithmType)) {
                if (Globals.MPHData[AlgorithmType].paying > paying) {
                    return MPHAddress;
                }
            }
            return BTCAddress;
        }
    }
}