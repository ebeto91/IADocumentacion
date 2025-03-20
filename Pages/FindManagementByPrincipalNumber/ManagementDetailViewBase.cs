using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.FindManagementByPrincipalNumber
{
    public class ManagementDetailViewBase : ComponentBase
    {
        [Parameter]
        public FullManagementDto ModelFirst { get; set; } = new FullManagementDto();
        [Parameter]
        public UserProfileResponse UserCreator { get; set; } = new UserProfileResponse();
        [Parameter]
        public List<Catalog> listCatalogData { get; set; } = new List<Catalog>();


        [Parameter]
        public List<string> listImagesSelected { get; set; } = new List<string>();

        [NotNull]
        public ImagePreviewer? ImagePreviewerRef { get; set; }
        public string findStatus(string code)
        {
            var item = listCatalogData.FirstOrDefault(x => x.Code == code && x.Collection == "STATUS-MANAGEMENT");
            if (item != null)
            {
                return item.DisplayLabel;
            }
            else
            {
                return "Sin asignar";
            }
        }
        public string validateData(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                return data;
            }
            else
            {
                return "Sin información";
            }
        }
        public Task ShowImagePreviewer() => ImagePreviewerRef.Show();



        #region table

        [NotNull]
        public Table<ManagementAttachedDocumentDto>? Table { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<ManagementAttachedDocumentDto>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<ManagementAttachedDocumentDto> items = new List<ManagementAttachedDocumentDto>();
            if (ModelFirst.AttachedDocuments != null)
            {
                items = ModelFirst.AttachedDocuments;

                return new QueryData<ManagementAttachedDocumentDto>()
                {
                    Items = items,
                    TotalCount = items.Count(),
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
            else
            {
                return new QueryData<ManagementAttachedDocumentDto>()
                {
                    Items = items,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };

            }





        }

        #endregion
    }
}
