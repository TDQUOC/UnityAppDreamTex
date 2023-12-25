using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace Alta.Plugin
{
    using Debug = UnityEngine.Debug;
    public static class ProcessExtension
    {

        /// <summary>
        /// -i./inputs/1.jpg -filter_complex "eq=brightness=-0.05:saturation=1.3 [cur];[cur]curves=psfile=./acvs/closeup.acv,boxblur=lr=1.5:lp=1:cr=1:ar=1" ./outputs/2.jpg -i
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Process RunCMD(string exePath,string arguments="")
        {
            Debug.Log(exePath+": "+arguments);
            FileInfo f = new FileInfo(exePath);
            Process proc = new Process();
            proc.StartInfo.FileName = exePath;//Path.Combine(Application.streamingAssetsPath, "ffmpeg/ffmpeg.exe");
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WorkingDirectory = f.Directory.FullName;

            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;

            //proc.ErrorDataReceived += build_ErrorDataReceived;
            //proc.OutputDataReceived += build_OutDataReceived;
            proc.EnableRaisingEvents = true;
            proc.Start();

            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            return proc;
        }
    }
}
