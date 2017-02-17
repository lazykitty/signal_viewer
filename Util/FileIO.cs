using System;
using System.IO;
using System.Windows.Forms;

/*  <<<< Example of usage 1  >>>>
 *  OpenFileDialog openFileDialog = SelectFileToLoad("CFG Files|*.cfg|All files|*.*");
 *  if (openFileDialog!=null)
 *  {
 *      string filename = openFileDialog.FileName;
 *      ...
 *  }  
 * 
 * <<<< Example of usage 2  >>>>
 *  FolderBrowserDialog fbd = FileIO.SelectFolder();
 *  if (fbd!=null)
 *  {
 *      string[] fileList = Directory.GetFiles(fbd.SelectedPath);
 *      ...
 *  }  
*/

namespace Signal_viewer.Util
{
    internal class FileIO
    {
        /// <summary>
        /// Open file manager to select folder for file to be save
        /// </summary>
        /// <param name="filter">extention filename filter</param>
        /// <returns></returns>
        static internal SaveFileDialog SelectFileToSave(String filter)
        {
            // Open save window diaglog
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = filter;  //"CFG Files|*.cfg|All files|*.*";
            saveFile.FilterIndex = 1;
            saveFile.RestoreDirectory = true;
            saveFile.ShowHelp = true;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                return saveFile;
            }
            else
            {
                return null;
            }       
        }
        /// <summary>
        /// Open file manager to select file to be read
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        static internal OpenFileDialog SelectFileToLoad(String filter)
        {
            //Create a new instance of a OpenFileDialog box
            OpenFileDialog openFile = new OpenFileDialog();
            //Set the parameters for the open file dialog box
            openFile.Filter = filter;  //"CFG files (*.CFG)|*.cfg|All files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;

            // Open file to be ready to read
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                return openFile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Open file manager to select folder 
        /// </summary>
        /// <returns></returns>
        static internal FolderBrowserDialog SelectFolder()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {                    
                    return fbd;
                }
            }
            return null;
        }



        static internal bool SaveByteArrayToFile(Byte[] data)
        {
            Stream myStream = null;
            bool result=false;
            SaveFileDialog saveFile = SelectFileToSave("All files|*.*");
            if (saveFile != null)
            {
                if ((myStream = saveFile.OpenFile()) != null)
                {
                    // to write the stream
                    try
                    {
                        myStream.Write(data, 0, data.Length);
                        result = true;
                    }
                    catch
                    {
                        // MessageBox.Show("File has error. Original error: " + ex.Message);
                    }
                    myStream.Close();
                }
            }
            return result;
        }


        static internal Byte[] LaoadFileToByteArray(int len)
        {
            Stream myStream = null;           

            OpenFileDialog openFile = SelectFileToLoad("All files|*.*");
            if (openFile != null)
            {
                try
                {
                    if ((myStream = openFile.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            Byte[] data = new Byte[len];
                            myStream.Read(data, 0, len);
                            return data;
                        }
                    }  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File has error. Original error: " + ex.Message);
                    //DEBUG_OUT("File size is not matched!!!");             
                }
            }
            return new Byte[0];
        }

    }
}
