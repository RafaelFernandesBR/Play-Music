// Copyright(C) 2017-2019 NV Access Limited, Arnold Loubriat
// This file is covered by the GNU Lesser General Public License, version 2.1.
// See the file license.txt for more details.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NVAccess
{
    /// <summary>
    /// C# wrapper class for the NVDA controller client DLL.
    /// </summary>
    static class NVDA
    {
        [DllImport("nvdaControllerClient64.dll", CharSet = CharSet.Unicode)]
        private static extern int nvdaController_brailleMessage(string message);

        [DllImport("nvdaControllerClient64.dll")]
        private static extern int nvdaController_cancelSpeech();

        [DllImport("nvdaControllerClient64.dll", CharSet = CharSet.Unicode)]
        private static extern int nvdaController_speakText(string text);

        [DllImport("nvdaControllerClient64.dll")]
        private static extern int nvdaController_testIfRunning();

        /// <summary>
        /// Indicates whether NVDA is running.
        /// </summary>
        public static bool IsRunning => nvdaController_testIfRunning() == 0;

        /// <summary>
        /// Tells NVDA to braille a message.
        /// </summary>
        /// <param name="message">The message to braille.</param>
        public static void Braille(string message)
        {
            nvdaController_brailleMessage(message);
        }

        /// <summary>
        /// Tells NVDA to immediately stop speaking.
        /// </summary>
        public static void CancelSpeech()
        {
            nvdaController_cancelSpeech();
        }

        /// <summary>
        /// Tells NVDA to speak some text.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        /// <param name="interrupt">If true, NVDA will immediately speak the text, interrupting whatever it was speaking before.</param>
        public static void Speak(string text, bool interrupt = true)
        {
            if (interrupt)
            {
                CancelSpeech();
            }
            nvdaController_speakText(text);
        }
    }

}
