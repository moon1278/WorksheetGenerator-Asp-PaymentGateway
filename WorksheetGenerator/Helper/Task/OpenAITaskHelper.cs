using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.Text;
using WorksheetGenerator.Data;
using WorksheetGenerator.Helper.Logging;
using WorksheetGenerator.Models.SpecificationModels;
using WorksheetGenerator.Models.WorksheetModels.TaskModels;
using WorksheetGenerator.Models.WorksheetModels.TaskModels.General;
using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Helper.Task
{
    public class OpenAITaskHelper
    {
        LogHelper _logger = new LogHelper();
        private OpenAI_API.OpenAIAPI API;

        public OpenAITaskHelper()
        {
            API = new OpenAI_API.OpenAIAPI("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", engine: "text-davinci-003");
        }

        #region ZeitenFormAnhandSatz
        public async Task<UserInput> GetAIData_ZeitformAnhandSatz_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {

            List<ZeitformAnhandSatz> zeitenDesVerbens = new List<ZeitformAnhandSatz>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                int number = getSentenceCount_ONLY_SPEC(specifications, taskId);


                string toReplace = "<dynamic_1>";
                string replaceWith = "" + number;          

                query = query.Replace(toReplace, replaceWith);

                JSONReturn_ZeitformAnhandSatz data = await Get_ZeitformAnhandSatz_DataAsync(query, Int32.Parse(replaceWith), 1);

                if(data == null)
                {
                    return null;

                }
                zeitenDesVerbens = data.Data;

                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = data.Json;
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }   
            

            if (zeitenDesVerbens.Count == 0)
            {
                //TODO: Hinzufügen einer Error Aufgabe + Nachricht, das die Aufgabe nicht erfolgreich war. Ggfs. fallback Aufgabeninhalt hinzufügen
            }
           
            ZeitformAnhandSatz_Task zeitenDesVerbens_Task = new ZeitformAnhandSatz_Task();
            zeitenDesVerbens_Task.ZeitformAnhandSatz_Values = zeitenDesVerbens;
            
            //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
            userInput.Tasks.Add(zeitenDesVerbens_Task);
            _logger.Log("OpenAITaskHelper return");

            return userInput;
            
        }

        private async Task<JSONReturn_ZeitformAnhandSatz> Get_ZeitformAnhandSatz_DataAsync(string query, int expected, int tryed)
        {  
            if(tryed == 10)
            {
                _logger.Log("OpenAI-Zeiten von Verben-NO SUCCESS");

                return null;
            }


            var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);
            var json = "[{" + result.ToString();

            List<ZeitformAnhandSatz> zeitenDesVerbens = new List<ZeitformAnhandSatz>();

            try
            {
                zeitenDesVerbens = JsonConvert.DeserializeObject<List<ZeitformAnhandSatz>>(json);
            }catch (Exception ex)
            {
                return await Get_ZeitformAnhandSatz_DataAsync(query, expected, tryed++);
            }

            if (zeitenDesVerbens.Count != expected)
            {
                return await Get_ZeitformAnhandSatz_DataAsync(query, expected, tryed++);
            }

            JSONReturn_ZeitformAnhandSatz dataJSONReturn = new JSONReturn_ZeitformAnhandSatz();
            dataJSONReturn.Data = zeitenDesVerbens;
            dataJSONReturn.Json = json;

            _logger.Log("OpenAI-Zeiten von Verben-Succes at " + tryed);

            return dataJSONReturn;
        }
        #endregion

        #region ZeichnenSilbenBogen
        public async Task<UserInput> GetAIData_ZeichnenSilbenBoegen_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {

            List<ZeichnenSilbenBoegen> zeichnenSilbenBoegens = new List<ZeichnenSilbenBoegen>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }


                int number = getSentenceCount_ONLY_SPEC(specifications, taskId);

                //Qualitätsmodul
                for (int i = 0; i < number; i++)
                {

                    var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);
                    var json = "[{" + result.ToString();

                    ZeichnenSilbenBoegen zeichnenSilbenBoegen = new ZeichnenSilbenBoegen(result.ToString());

                    //1 index accessor
                    int strLength = zeichnenSilbenBoegen.Begriff.Length;

                    for (int j = 0; j < strLength; j++)
                    {
                        if (zeichnenSilbenBoegen.Begriff[j] == '-')
                        {
                            zeichnenSilbenBoegen.SilbenPosition.Add(j);
                        }
                    }

                    zeichnenSilbenBoegens.Add(zeichnenSilbenBoegen);
                }
                                      
                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(zeichnenSilbenBoegens); //TODO change tostring to json
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }

            if (zeichnenSilbenBoegens.Count == 0)
            {
                //TODO: Hinzufügen einer Error Aufgabe + Nachricht, das die Aufgabe nicht erfolgreich war. Ggfs. fallback Aufgabeninhalt hinzufügen
            }

            ZeichnenSilbenBoegen_Task zeichnenSilbenBoegen_Task = new ZeichnenSilbenBoegen_Task();
            zeichnenSilbenBoegen_Task.ZeichnenSilbenBoegen_Values = zeichnenSilbenBoegens;

            //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
            userInput.Tasks.Add(zeichnenSilbenBoegen_Task);
            _logger.Log("OpenAITaskHelper return");

            return userInput;

        }
        #endregion

        #region SatzAnfangAustauschen
        public async Task<UserInput> GetAIData_SatzAnfangAustauschen_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {

            SingleStringReturn stringText = new SingleStringReturn();
            stringText.TaskId = taskId;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                int number = 0;
            
                List<SpecificationReplaceModel> specificationsList = new List<SpecificationReplaceModel>();
                int sentenceAmount = 0;
                string theme = "Tagesablauf von Emma.";

                foreach (var specification in specifications)
                {
                    WorksheetGenerator.Data.Specification spec = db.Specifications.Where(x => x.Id == Int32.Parse(specification.SpecificationId)).FirstOrDefault();

                    if (spec.TaskId != taskId)
                    {
                        continue;
                    }

                    switch (spec.Dynamic_Replace_Text)
                    {
                        case "<dynamic_1>":
                            sentenceAmount = Int32.Parse(specification.Value);

                            break;
                        case "<dynamic_2>":
                            theme = specification.Value;

                            break;
                    }               
                }           

                //Qualitätsmodul

                string toReplace = "<dynamic_1>";
                string replaceWith = "" + sentenceAmount;
                query = query.Replace(toReplace, replaceWith);

                toReplace = "<dynamic_2>";
                replaceWith = theme;
                query = query.Replace(toReplace, replaceWith);


                var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);

                stringText.Text = result.ToString();
                
                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(stringText); //TODO change tostring to json
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }

            SingleStringReturn_Task singleStringReturn_Task = new SingleStringReturn_Task();
            singleStringReturn_Task.SingleStringReturn_Value = stringText;

            //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
            userInput.Tasks.Add(singleStringReturn_Task);

            return userInput;

        }
        #endregion

        #region PassendeSatzanfaengefinden
        public async Task<UserInput> GetAIData_PassendeSatzanfaengefinden_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {


            List<SingleStringReturn> stringList = new List<SingleStringReturn>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                int number = getSentenceCount_ONLY_SPEC(specifications, taskId);

                for(int i = 0; i< number; i++)
                {
                    SingleStringReturn stringText = new SingleStringReturn();
                    stringText.TaskId = taskId;


                    string toReplace = "<dynamic_1>";
                    string replaceWith = "" + number;

                    query = query.Replace(toReplace, replaceWith);

                    var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);

                    stringText.Text = result.ToString();

                    stringList.Add(stringText);
                }
                

                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(stringList); //TODO change tostring to json
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }

            ListStringReturn_Task listStringReturn_Task = new ListStringReturn_Task();
            listStringReturn_Task.StringList_Values = stringList;

            //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
            userInput.Tasks.Add(listStringReturn_Task);

            return userInput;

        }
        #endregion

        #region SatzAnfangAustauschen
        public async Task<UserInput> GetAIData_SatzZeitUmschreiben_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {
            List<SatzZeitUmschreiben> sentences_and_solutions = new List<SatzZeitUmschreiben>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                int number = 0;

                List<SpecificationReplaceModel> specificationsList = new List<SpecificationReplaceModel>();
                int amountPräsens = 0;
                int amountPräteritum = 0;
                int amountPerfekt = 0;
                int amountFuture = 0;

                foreach (var specification in specifications)
                {
                    WorksheetGenerator.Data.Specification spec = db.Specifications.Where(x => x.Id == Int32.Parse(specification.SpecificationId)).FirstOrDefault();

                    if (spec.TaskId != taskId)
                    {
                        continue;
                    }

                    switch (spec.Dynamic_Replace_Text)
                    {
                        case "<dynamic_1>":
                            amountPräsens = Int32.Parse(specification.Value);

                            break;
                        case "<dynamic_2>":
                            amountPräteritum = Int32.Parse(specification.Value);

                            break;
                        case "<dynamic_3>":
                            amountPerfekt = Int32.Parse(specification.Value);

                            break;
                        case "<dynamic_4>":
                            amountFuture = Int32.Parse(specification.Value);

                            break;
                    }
                }

                for (int i = 0; i < amountPräsens; i++)
                {

                    var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);
                    SatzZeitUmschreiben_Json satzZeitUmschreiben = JsonConvert.DeserializeObject<SatzZeitUmschreiben_Json>("{"+result.ToString());

                    sentences_and_solutions.Add(getSolutionAndSentences(satzZeitUmschreiben, ZeitFormen.Praesens));
                }

                for (int i = 0; i < amountFuture; i++)
                {

                    var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 700);
                    SatzZeitUmschreiben_Json satzZeitUmschreiben = JsonConvert.DeserializeObject<SatzZeitUmschreiben_Json>("{"+result.ToString());

                    sentences_and_solutions.Add(getSolutionAndSentences(satzZeitUmschreiben, ZeitFormen.Future));
                }

                for (int i = 0; i < amountPerfekt; i++)
                {

                    var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 700);
                    SatzZeitUmschreiben_Json satzZeitUmschreiben = JsonConvert.DeserializeObject<SatzZeitUmschreiben_Json>("{"+result.ToString());

                    sentences_and_solutions.Add(getSolutionAndSentences(satzZeitUmschreiben, ZeitFormen.Perfekt));
                }

                for (int i = 0; i < amountPräteritum; i++)
                {

                    var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 700);
                    SatzZeitUmschreiben_Json satzZeitUmschreiben = JsonConvert.DeserializeObject<SatzZeitUmschreiben_Json>("{"+result.ToString());

                    sentences_and_solutions.Add(getSolutionAndSentences(satzZeitUmschreiben, ZeitFormen.Praeteritum));
                }

                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(sentences_and_solutions); //TODO change tostring to json
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }

            SatzZeitUmschreiben_Task satzZeitUmschreiben_Task = new SatzZeitUmschreiben_Task();
            satzZeitUmschreiben_Task.SatzZeitUmschreiben_Values = sentences_and_solutions;

            //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
            userInput.Tasks.Add(satzZeitUmschreiben_Task);

            return userInput;

        }
        #endregion

        #region ArtikelEinsaetzen
        public async Task<UserInput> GetAIData_ArtikelEinsaetzen_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {

            SingleStringReturn stringText = new SingleStringReturn();
            stringText.TaskId = taskId;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                int number = 0;

                List<SpecificationReplaceModel> specificationsList = new List<SpecificationReplaceModel>();
                int sentenceAmount = 0;
                string theme = "Tagesablauf von Emma.";

                foreach (var specification in specifications)
                {
                    WorksheetGenerator.Data.Specification spec = db.Specifications.Where(x => x.Id == Int32.Parse(specification.SpecificationId)).FirstOrDefault();

                    if (spec.TaskId != taskId)
                    {
                        continue;
                    }

                    switch (spec.Dynamic_Replace_Text)
                    {
                        case "<dynamic_2>":
                            sentenceAmount = Int32.Parse(specification.Value);

                            break;
                        case "<dynamic_1>":
                            theme = specification.Value;

                            break;
                    }
                }


                string toReplace = "<dynamic_1>";
                string replaceWith = "" + sentenceAmount;
                query = query.Replace(toReplace, replaceWith);

                toReplace = "<dynamic_2>";
                replaceWith = theme;
                query = query.Replace(toReplace, replaceWith);


                var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);

                stringText.Text = result.ToString();

                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(stringText); //TODO change tostring to json
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }

            SingleStringReturn_Task singleStringReturn_Task = new SingleStringReturn_Task();
            singleStringReturn_Task.SingleStringReturn_Value = stringText;

            userInput.Tasks.Add(singleStringReturn_Task);

            return userInput;

        }
        #endregion

        #region AddiereZahlen100Bis1000
        public async Task<UserInput> GetAIData_AddiereZahlen100Bis1000_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {

            List<AddiereZahlen100Bis1000> addiereZahlen100Bis1000List = new List<AddiereZahlen100Bis1000>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                int taskAmount = 0;

                List<SpecificationReplaceModel> specificationsList = new List<SpecificationReplaceModel>();
                foreach (var specification in specifications)
                {
                    WorksheetGenerator.Data.Specification spec = db.Specifications.Where(x => x.Id == Int32.Parse(specification.SpecificationId)).FirstOrDefault();

                    if (spec.TaskId != taskId)
                    {
                        continue;
                    }

                    switch (spec.Dynamic_Replace_Text)
                    {
                        case "<dynamic_1>":
                            taskAmount = Int32.Parse(specification.Value);
                            break;
                        default:
                            break;
                    }
                }

                string toReplace = "<dynamic_1>";
                string replaceWith = "" + taskAmount;

                query = query.Replace(toReplace, replaceWith);

                //Qualitätsmodul


                var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);

                var test = "[\"" + result.ToString();

                List<string> stringList = JsonConvert.DeserializeObject<List<string>>(test); 


                foreach( var assignment in stringList) {
                    if (assignment != null)
                    {
                        string[] words = assignment.Split(" = ");
                        AddiereZahlen100Bis1000 addiereZahlen100Bis1000 = new AddiereZahlen100Bis1000(_aufgabe : words[0], _loesung : words[1]);
                        addiereZahlen100Bis1000List.Add(addiereZahlen100Bis1000);
                    }
                }
                   

                
                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(addiereZahlen100Bis1000List);
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }


            if (addiereZahlen100Bis1000List.Count == 0)
            {
                //TODO: Hinzufügen einer Error Aufgabe + Nachricht, das die Aufgabe nicht erfolgreich war. Ggfs. fallback Aufgabeninhalt hinzufügen
            }

            AddiereZahlen100Bis1000_Task addiereZahlen100Bis1000_Task = new AddiereZahlen100Bis1000_Task();
            addiereZahlen100Bis1000_Task.AddiereZahlen100Bis1000_Values = addiereZahlen100Bis1000List;

            userInput.Tasks.Add(addiereZahlen100Bis1000_Task);


            return userInput;

        }
        #endregion

        #region GegenteileFinden
        public async Task<UserInput> GetAIData_GegenteileFinden_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {
            //Initzialisieren der erstellen Task Klasse (wird benutzt um die Werte in die Datenbank zu speichern)
            ListArrayReturn gegenteileFindenListe = new ListArrayReturn();

            //Get Task Information
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                //Definiere der Objekte die für die Spezifikationen gebraucht werden (n-Anzahl)
                int gegenteilAnzahl = 0;

                //Get Spezifikationen Werte
                List<SpecificationReplaceModel> specificationsList = new List<SpecificationReplaceModel>();
                foreach (var specification in specifications)
                {
                    WorksheetGenerator.Data.Specification spec = db.Specifications.Where(x => x.Id == Int32.Parse(specification.SpecificationId)).FirstOrDefault();

                    if (spec.TaskId != taskId)
                    {
                        continue;
                    }

                    //Dynmics müssen übereinstimmen
                    switch (spec.Dynamic_Replace_Text)
                    {
                        case "<dynamic_1>":
                            gegenteilAnzahl = Int32.Parse(specification.Value);
                            break;
                        default:
                            break;
                    }
                }

                
                var api = new OpenAI_API.OpenAIAPI("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", engine: "text-davinci-003");

                //Dynamic Werte in Query übertragen
                string toReplace = "<dynamic_1>";
                string replaceWith = "" + gegenteilAnzahl;
                query = query.Replace(toReplace, replaceWith);

                //Get KI Query Data
                var result = await api.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);

                //Manipulation der Daten in das Richtige Format
                var test = "[" + result.ToString();

                //Befüllen der Task Klasse (oben Erstellt)
                gegenteileFindenListe.ListArrayReturnValue = JsonConvert.DeserializeObject<List<string[]>>(test);
                gegenteileFindenListe.TaskId = taskId;

                //Task speichern in die Datenbank
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(gegenteileFindenListe); //Speichern der gerade erstellen Task Klasse
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();
            }

            ListArrayReturn_Task gegenteilFinden_Task = new ListArrayReturn_Task();
            gegenteilFinden_Task.ListArray_Values = gegenteileFindenListe;

            userInput.Tasks.Add(gegenteilFinden_Task);

            return userInput;

        }
        #endregion

        #region ArtikelVorNomen
        public async Task<UserInput> GetAIData_ArtikelVorNomen_Async(UserInput userInput, int worksheetId, int taskId, List<SpecificationValueModel> specifications)
        {
            List<SingleStringReturn> stringList = new List<SingleStringReturn>();
            int sentenceAmount = 0;
            string theme = "";
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                int s = db.Tasks.ToList().Count;
                string query = task.Query;

                //WENN QUERY == null, kann es daran liegen das in testzwecken keine query in der datenbank liegt
                if (query == null)
                {
                    return null;
                }

                foreach (var specification in specifications)
                {
                    WorksheetGenerator.Data.Specification spec = db.Specifications.Where(x => x.Id == Int32.Parse(specification.SpecificationId)).FirstOrDefault();

                    if (spec.TaskId != taskId)
                    {
                        continue;
                    }

                    switch (spec.Dynamic_Replace_Text)
                    {
                        case "<dynamic_1>":
                            sentenceAmount = Int32.Parse(specification.Value);

                            break;
                        case "<dynamic_2>":
                            theme = specification.Value;

                            break;
                    }
                }

               

                string toReplace = "<dynamic_1>";
                string replaceWith = "" + sentenceAmount;

                query = query.Replace(toReplace, replaceWith);

                toReplace = "<dynamic_2>";
                replaceWith = theme;

                query = query.Replace(toReplace, replaceWith);

                var result = await API.Completions.CreateCompletionAsync(query, temperature: 0.7, max_tokens: 2000);

                var json = "[\"d" + result.ToString();

                List<string> sentences = JsonConvert.DeserializeObject<List<string>>(json);

                foreach (string sentence in sentences)
                {
                    SingleStringReturn sString = new SingleStringReturn();
                    sString.TaskId = taskId;
                    sString.Text = sentence;

                    stringList.Add(sString);
                }
                //TODO SAFE RESULT OF TASK
                RlWorksheetTask rlWorksheetTask = new RlWorksheetTask();
                rlWorksheetTask.WorksheetId = worksheetId;
                rlWorksheetTask.TaskId = taskId;
                rlWorksheetTask.Result = JsonConvert.SerializeObject(stringList); //TODO change tostring to json
                rlWorksheetTask.Created = DateTime.UtcNow; //TODO Add time

                db.RlWorksheetTasks.Add(rlWorksheetTask);
                db.SaveChanges();

            }

            ListStringReturn_Task listStringReturn_Task = new ListStringReturn_Task();
            listStringReturn_Task.StringList_Values = stringList;

            //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
            userInput.Tasks.Add(listStringReturn_Task);

            if (true)
            {
                string url = "https://api.synthesia.io/v2/videos/fromTemplate";
                string auth = "720127c86cf36dc8173cc90fe162363a";
                string json = "{\"templateId\": \"c5c84301-28a7-401b-8b44-baf4de214581\", \"templateData\": {\"name\": \"Henry\", \"GPT3_Input\": \"Schülerinnen und Schüler bekommen ein Blatt mit verschiedenen Nomen. Anstatt der Artikel, steht vor den Nomen ein Leerzeichen. Schülerinnen und Schüler sollen die Artikel der Nomen herausfinden und diese anschließend in eine Tabelle mit der, die und das eintragen\", \"test\": true}}";

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Post,
                        Content = new StringContent(json, Encoding.UTF8, "application/json")

                    };
                    request.Headers.Add("Authorization", auth);
                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                }

            }


            return userInput;

        }
        #endregion

        //CAUTION ONLY USE IF TASK HAS ONE SPECIFICATION
        private int getSentenceCount_ONLY_SPEC(List<SpecificationValueModel> specifications, int taskId)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                List<SpecificationReplaceModel> specificationsList = new List<SpecificationReplaceModel>();

                foreach (var specification in specifications)
                {
                    WorksheetGenerator.Data.Specification spec = db.Specifications.Where(x => x.Id == Int32.Parse(specification.SpecificationId)).FirstOrDefault();
                    int sentenceAmount = 0;

                    if (spec.TaskId == taskId)
                    {
                        sentenceAmount = Int32.Parse(specification.Value);
                    }

                    SpecificationReplaceModel obj = new SpecificationReplaceModel();

                    obj.Specification = spec;
                    obj.Value = sentenceAmount.ToString();

                    specificationsList.Add(obj);
                }

                int number = int.Parse(specificationsList.First().Value);

                return number;
            }
        }

        private SatzZeitUmschreiben getSolutionAndSentences(SatzZeitUmschreiben_Json satzZeitUmschreiben, ZeitFormen solution)
        {
            SatzZeitUmschreiben sentence_solution = new SatzZeitUmschreiben();
            List<SatzZeitUmschreiben_Instance> satzZeitUmschreiben_Instances = new List<SatzZeitUmschreiben_Instance>();

            switch (solution)
            {
                case ZeitFormen.Praesens:
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praesens = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praesens, true, ZeitFormen.Praesens);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praeteritum = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praeteritum, false, ZeitFormen.Praeteritum);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_future = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Future, false, ZeitFormen.Future);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_perfekt = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Perfekt, false, ZeitFormen.Perfekt);
                    
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praesens);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praeteritum);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_future);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_perfekt);
                    break;
                case ZeitFormen.Perfekt:
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praesens_1 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praesens, false, ZeitFormen.Praesens);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praeteritum_1 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praeteritum, false, ZeitFormen.Praeteritum);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_future_1 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Future, false, ZeitFormen.Future);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_perfekt_1 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Perfekt, true, ZeitFormen.Perfekt);

                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praesens_1);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praeteritum_1);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_future_1);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_perfekt_1);
                    break;
                    
                case ZeitFormen.Praeteritum:
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praesens_2 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praesens, false, ZeitFormen.Praesens);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praeteritum_2 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praeteritum, true, ZeitFormen.Praeteritum);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_future_2 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Future, false, ZeitFormen.Future);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_perfekt_2 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Perfekt, false, ZeitFormen.Perfekt);

                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praesens_2);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praeteritum_2);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_future_2);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_perfekt_2);
                    break;
                case ZeitFormen.Future:
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praesens_3 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praesens, false, ZeitFormen.Praesens);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_praeteritum_3 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Praeteritum, false, ZeitFormen.Praeteritum);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_future_3 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Future, true, ZeitFormen.Future);
                    SatzZeitUmschreiben_Instance satzZeitUmschreiben_perfekt_3 = new SatzZeitUmschreiben_Instance(satzZeitUmschreiben.Perfekt, false, ZeitFormen.Perfekt);

                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praesens_3);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_praeteritum_3);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_future_3);
                    satzZeitUmschreiben_Instances.Add(satzZeitUmschreiben_perfekt_3);
                    break;
            }

      
            sentence_solution.SentenceList = satzZeitUmschreiben_Instances;

            return sentence_solution;
        }
    }
}
