using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.InstitutionalMemory;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface ISurveyService
    {
        Task<GetSurveyFilterResponse> GetSurveyFilter(SurveyInputExternalDto input);
        Task<GetSurveyListAdminstratorResponseDefinition> GetSurveyAdminstratorFilter(SurveyInputDto input);

        Task<GetSurveyItemResponseDefinition> PostSurveyPending(HandleSurveyConfig input);
        Task<GetSurveyItemResponseDefinition> PostSurveyAproveDirect(HandleSurveyConfig input);
        Task<GetSurveyItemResponseDefinition> PostSurveyChangeStatus(ChangeStatusSurveyDto input);
        Task<GetSurveyItemResponseDefinition> PutSurvey(HandleSurveyConfig input);
        Task<GetSurveyItemResponseDefinition> GetSurveyById(string id);

        Task<ResponseModelQuestionSurveyVote> GetSurveyForVoteById(string id);

        Task<ResponseModelModelSurveyVote> GetSurveyExternalsFilter(SurveyInputDto input);

        Task<ResponseModel> SendSurveyOrVote(RegisterVoteSurveyListDto input);

        Task<ResultSurveyVote> ShowResultsSurvey(ShowResultsInputDto input);
    }

    class SurveyService : ISurveyService
    {

        public HttpClient HttpClient { get; }
        private const long MaxFileSize = 10240000L;

        public SurveyService(HttpClient httpClient)
        {
            HttpClient = httpClient;

        }
        public async Task<GetSurveyFilterResponse> GetSurveyFilter(SurveyInputExternalDto input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Survey/GetSurveyExternalsPublicFilter", input);

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetSurveyFilterResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<GetSurveyListAdminstratorResponseDefinition> GetSurveyAdminstratorFilter(SurveyInputDto input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Survey/GetSurveyAdminstratorFilter", input);

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetSurveyListAdminstratorResponseDefinition>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }


        public async Task<ResponseModelModelSurveyVote> GetSurveyExternalsFilter(SurveyInputDto input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Survey/GetSurveyExternalsFilter", input);

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ResponseModelModelSurveyVote>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<GetSurveyItemResponseDefinition> PostSurveyPending(HandleSurveyConfig input)
        {
            try
            {
                using var handleSurveyData = new MultipartFormDataContent();
                LoadDataToFormDataContentPostSurvey(input, handleSurveyData);

                var response = await HttpClient.PostAsync($"/api/Survey/PostSurveyPending", handleSurveyData);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetSurveyItemResponseDefinition>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<GetSurveyItemResponseDefinition> PostSurveyAproveDirect(HandleSurveyConfig input)
        {

            try
            {
                using var handleSurveyData = new MultipartFormDataContent();
                LoadDataToFormDataContentPostSurvey(input, handleSurveyData);

                var response = await HttpClient.PostAsync($"/api/Survey/PostSurveyAproveDirect", handleSurveyData);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetSurveyItemResponseDefinition>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }

        }

        private static void LoadDataToFormDataContentPostSurvey(HandleSurveyConfig input, MultipartFormDataContent handleSurveyData)
        {
            // Use reflection to iterate over the properties of formData
            foreach (var property in input.GetType().GetProperties())
            {
                var value = property.GetValue(input);
                if (value != null)
                {


                    if (property.Name == "StartDate" || property.Name == "DueRate")
                    {
                        var valueDate = (DateTime)value;
                        var format = valueDate.ToString("yyyy-MM-dd");
                        handleSurveyData.Add(new StringContent(format), property.Name);
                    }
                    else
                    {
                        if (property.Name != "AttachedNewFiles" && property.Name != "SurveyQuestions" && property.Name != "NewPrincipalImage" && property.Name != "SurveyAttachedDocuments")
                        {
                            handleSurveyData.Add(new StringContent(value?.ToString()), property.Name);
                        }

                    }
                }
            }

            if (input.NewPrincipalImage != null)
            {
                var file = input.NewPrincipalImage;
                var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                handleSurveyData.Add(fileStreamContent, "NewPrincipalImage", file.Name);
            }

            if (input.AttachedNewFiles != null)
            {
                // Add files to form data
                foreach (var file in input.AttachedNewFiles)
                {
                    var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    handleSurveyData.Add(fileStreamContent, "AttachedNewFiles", file.Name);
                }
            }
            //lista de preguntas
            if (input.SurveyQuestions != null)
            {
                //obtener nombre de la primer lista
                var listName = nameof(input.SurveyQuestions);
                for (int i = 0; i < input.SurveyQuestions.Count(); i++)
                {
                    //item en la lista
                    var item = input.SurveyQuestions[i];
                    //obtener las propiedades
                    foreach (var property in item.GetType().GetProperties())
                    {
                        //obtener el valor
                        var value = property.GetValue(item);
                        //validar
                        if (value != null)
                        {
                            //si es la lista de preguntas hacemos un recorrido para poder agregarlo
                            if (property.Name == "SurveyQuestionOptions")
                            {
                                //obtener el nombre
                                var listQuestionOptions = nameof(item.SurveyQuestionOptions);
                                //recorrer la lista de respuestas
                                for (int j = 0; j < item.SurveyQuestionOptions.Count(); j++)
                                {
                                    //obtener el item de las respuestas
                                    var itemQuestionOptions = item.SurveyQuestionOptions[j];
                                    //obtener las propiedades
                                    foreach (var propertyQuestionOption in itemQuestionOptions.GetType().GetProperties())
                                    {
                                        //obtener el valor
                                        var valueQuestionOption = propertyQuestionOption.GetValue(itemQuestionOptions);
                                        if (valueQuestionOption != null)
                                        {
                                            if (propertyQuestionOption.Name == "NewFileQuestion")
                                            {
                                                if (itemQuestionOptions.NewFileQuestion != null)
                                                {
                                                    var file = itemQuestionOptions.NewFileQuestion;
                                                    var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                                                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                                                    var name = $"{listName}[{i}].{listQuestionOptions}[{j}].{propertyQuestionOption.Name}";
                                                    handleSurveyData.Add(fileStreamContent, name, file.Name);
                                                }

                                            }
                                            else
                                            {
                                                var name = $"{listName}[{i}].{listQuestionOptions}[{j}].{propertyQuestionOption.Name}";
                                                handleSurveyData.Add(new StringContent(valueQuestionOption?.ToString()), name);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var name = $"{listName}[{i}].{property.Name}";
                                handleSurveyData.Add(new StringContent(value?.ToString()), name);
                            }
                        }
                    }
                }
            }
        }

        public async Task<GetSurveyItemResponseDefinition> PostSurveyChangeStatus(ChangeStatusSurveyDto input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Survey/PostSurveyChangeStatus", input);

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetSurveyItemResponseDefinition>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<GetSurveyItemResponseDefinition> PutSurvey(HandleSurveyConfig input)
        {
            try
            {
                using var handleSurveyData = new MultipartFormDataContent();
                LoadDataToFormDataContentPostSurveyEdit(input, handleSurveyData);

                var response = await HttpClient.PostAsync($"/api/Survey/PutSurvey", handleSurveyData);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetSurveyItemResponseDefinition>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }
        private static void LoadDataToFormDataContentPostSurveyEdit(HandleSurveyConfig input, MultipartFormDataContent handleSurveyData)
        {
            // Use reflection to iterate over the properties of formData
            foreach (var property in input.GetType().GetProperties())
            {
                var value = property.GetValue(input);
                if (value != null)
                {
                    if (property.Name == "StartDate" || property.Name == "DueRate")
                    {
                        var valueDate = (DateTime)value;
                        var format = valueDate.ToString("yyyy-MM-dd");
                        handleSurveyData.Add(new StringContent(format), property.Name);
                    }
                    else
                    {
                        if (property.Name != "AttachedNewFiles" && property.Name != "SurveyQuestions" && property.Name != "PrincipalImage" && property.Name != "NewPrincipalImage" && property.Name != "SurveyAttachedDocuments")
                        {
                            handleSurveyData.Add(new StringContent(value?.ToString()), property.Name);
                        }
                    }
                }
            }

            if (input.NewPrincipalImage != null)
            {
                var file = input.NewPrincipalImage;
                var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                handleSurveyData.Add(fileStreamContent, "NewPrincipalImage", file.Name);
            }

            if (input.AttachedNewFiles != null)
            {
                // Add files to form data
                foreach (var file in input.AttachedNewFiles)
                {
                    var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    handleSurveyData.Add(fileStreamContent, "AttachedNewFiles", file.Name);
                }
            }


            if (input.SurveyAttachedDocuments != null)
            {
                var listName = nameof(input.SurveyAttachedDocuments);
                // Add files to form data
                //foreach (var file in input.SurveyAttachedDocuments)
                //{
                for (int i = 0; i < input.SurveyAttachedDocuments.Count; i++)
                {
                    var file = input.SurveyAttachedDocuments[i];

                    foreach (var property in file.GetType().GetProperties())
                    {
                        var value = property.GetValue(file);
                        if (value != null)
                        {
                            var name = $"{listName}[{i}].{property.Name}";
                            handleSurveyData.Add(new StringContent(value?.ToString()), name);
                        }
                    }
                }


                //}
            }

            if (input.PrincipalImage != null)
            {
                // Add files to form data
                foreach (var property in input.PrincipalImage.GetType().GetProperties())
                {
                    var value = property.GetValue(input.PrincipalImage);
                    if (value != null)
                    {
                        var principalImage = nameof(input.PrincipalImage);
                        var name = $"{principalImage}.{property.Name}";
                        handleSurveyData.Add(new StringContent($"{value?.ToString()}"), name);
                    }
                }


            }
            //lista de preguntas
            if (input.SurveyQuestions != null)
            {
                //obtener nombre de la primer lista
                var listName = nameof(input.SurveyQuestions);
                for (int i = 0; i < input.SurveyQuestions.Count(); i++)
                {
                    //item en la lista
                    var item = input.SurveyQuestions[i];
                    //obtener las propiedades
                    foreach (var property in item.GetType().GetProperties())
                    {
                        //obtener el valor
                        var value = property.GetValue(item);
                        //validar
                        if (value != null)
                        {
                            //si es la lista de preguntas hacemos un recorrido para poder agregarlo
                            if (property.Name == "SurveyQuestionOptions")
                            {
                                //obtener el nombre
                                var listQuestionOptions = nameof(item.SurveyQuestionOptions);
                                //recorrer la lista de respuestas
                                for (int j = 0; j < item.SurveyQuestionOptions.Count(); j++)
                                {
                                    //obtener el item de las respuestas
                                    var itemQuestionOptions = item.SurveyQuestionOptions[j];
                                    //obtener las propiedades
                                    foreach (var propertyQuestionOption in itemQuestionOptions.GetType().GetProperties())
                                    {
                                        //obtener el valor
                                        var valueQuestionOption = propertyQuestionOption.GetValue(itemQuestionOptions);
                                        if (valueQuestionOption != null)
                                        {
                                            if (propertyQuestionOption.Name == "NewFileQuestion")
                                            {
                                                if (itemQuestionOptions.NewFileQuestion != null)
                                                {
                                                    var file = itemQuestionOptions.NewFileQuestion;
                                                    var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                                                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                                                    var name = $"{listName}[{i}].{listQuestionOptions}[{j}].{propertyQuestionOption.Name}";
                                                    handleSurveyData.Add(fileStreamContent, name, file.Name);
                                                }

                                            }
                                            else
                                            {
                                                if (propertyQuestionOption.Name == "SurveyQuestionOptionDocuments")
                                                {
                                                    if (itemQuestionOptions.SurveyQuestionOptionDocuments != null)
                                                    {
                                                        var itemSurveyQuestionOptionDocumentVal = itemQuestionOptions.SurveyQuestionOptionDocuments;

                                                        foreach (var itemSurveyQuestionOptionDocumentsProps in itemSurveyQuestionOptionDocumentVal.GetType().GetProperties())
                                                        {
                                                            var itemSurveyQuestionOptionDocumentsValue = itemSurveyQuestionOptionDocumentsProps.GetValue(itemSurveyQuestionOptionDocumentVal);
                                                            if (itemSurveyQuestionOptionDocumentsValue != null)
                                                            {
                                                                var name = $"{listName}[{i}].{listQuestionOptions}[{j}].{propertyQuestionOption.Name}.{itemSurveyQuestionOptionDocumentsProps.Name}";
                                                                handleSurveyData.Add(new StringContent(itemSurveyQuestionOptionDocumentsValue?.ToString()), name);
                                                            }
                                                        }
                                                    }
                                                  
                                                }
                                                else
                                                {
                                                    var name = $"{listName}[{i}].{listQuestionOptions}[{j}].{propertyQuestionOption.Name}";
                                                    handleSurveyData.Add(new StringContent(valueQuestionOption?.ToString()), name);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var name = $"{listName}[{i}].{property.Name}";
                                handleSurveyData.Add(new StringContent(value?.ToString()), name);
                            }
                        }
                    }
                }
            }
        }


        public async Task<ResponseModelQuestionSurveyVote> GetSurveyForVoteById(string id)
        {
            try
            {
                var response = await HttpClient.GetAsync($"/api/Survey/GetSurveyForVoteById/{id}");

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ResponseModelQuestionSurveyVote>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<GetSurveyItemResponseDefinition> GetSurveyById(string id)
        {
            try
            {

                var response = await HttpClient.GetAsync($"/api/Survey/GetSurveyById/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetSurveyItemResponseDefinition>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<ResponseModel> SendSurveyOrVote(RegisterVoteSurveyListDto input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Survey/RegisterVoteSurvey", input);

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ResponseModel>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<ResultSurveyVote> ShowResultsSurvey(ShowResultsInputDto input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Survey/ShowResultsSurvey", input);

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ResultSurveyVote>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }
    }
}
