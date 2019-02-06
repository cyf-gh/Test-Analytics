using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Test_Analytics.Model;
using Test_Analytics.Service;

namespace Test_Analytics.Web.Controllers {


    [Route( "/project" )]
    public class HomeController : Controller {
        private readonly ITestAnalyticsService _TestAnalyticsService;

        public class XLS {
            public string Name { get; set; }
            public string tableHTML { get; set; }
        }

        public HomeController( ITestAnalyticsService testAnalyticsService ) {
            _TestAnalyticsService = testAnalyticsService;
        }

        [HttpGet]
        public async Task<IActionResult> OnEditProject( int id ) {
            if( id == 0 ) { return BadRequest(); }

            var ProjectSet = _TestAnalyticsService.ProjectSet();
            var CurrentProject = await ProjectSet.FindAsync( id );

            if( CurrentProject == null ) { return NotFound(); }

            return View( "edit", await _TestAnalyticsService.GetViewModelByProject( CurrentProject ) );
        }
        [HttpGet( "learn-more" )]
        public IActionResult OnGetLearnMore() {
            return View( "LearnMore" );
        }
        [HttpGet( "index" )]
        public async Task<IActionResult> OnGetIndex() {
            return View( "Index", await _TestAnalyticsService.GetProjectsAsync() );
        }
        [HttpPost( "make-risktable-xls" )]
        public IActionResult MakeRiskTable() {
            XLS xls = Utils.GetObjectFromJsonInRequest<XLS>( Request );
            string path = Path.Combine( Environment.CurrentDirectory, ( xls.Name + "_Risk.xls" ) );
            if( System.IO.File.Exists( path ) ) {
                System.IO.File.Delete( path );
            }
            System.IO.File.WriteAllText( path, xls.tableHTML );
            return Ok();
        }

        [HttpGet( "download-risktable-xls" )]
        public IActionResult DownloadRiskTable(string file) {
            string path = Path.Combine( Environment.CurrentDirectory, file );
            if( System.IO.File.Exists( path ) ) {
                var stream = System.IO.File.OpenRead( path );
                return File( stream, "application/vnd.ms-excel", file );
            }
            return NotFound(Content("Post url'project/make-risktable-xls' and try again."));
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject() {
            try {
                await _TestAnalyticsService.ProjectSet()
                    .AddAsync( Utils.GetObjectFromJsonInRequest<ProjectModel>( Request ) );
                await _TestAnalyticsService.SaveAsync();
                return Json( await _TestAnalyticsService.GetProjectsAsync() );
            } catch( System.Exception ) {
                return BadRequest();
            }
        }
        [HttpPost( "save-project-capas" )]
        public async Task<IActionResult> SaveProjectCapabilities() {
            try {
                await _TestAnalyticsService.SaveCapabilitiesAsync( Utils.GetObjectFromJsonInRequest<List<PostCapa>>( Request ) );
                await _TestAnalyticsService.SaveAsync();
                return Json( await _TestAnalyticsService.GetCapabilitiesAsync() );
            } catch( System.Exception ) {
                return BadRequest();
            }
        }
        [HttpPost( "delete-project" )]
        public async Task<IActionResult> PushSave() {
            try {
                var project = Utils.GetObjectFromJsonInRequest<ProjectModel>( Request );
                var projectToDelete = ( await _TestAnalyticsService.GetProjectsAsync() ).Find( p => { return p.Id == project.Id; } );
                _TestAnalyticsService.DeleteAllCapabilityByProject( await _TestAnalyticsService.GetAllCapabilitiesByProject( projectToDelete ) );
                _TestAnalyticsService.DeleteAllAttributesByProject( await _TestAnalyticsService.GetAllAttributesByProject( projectToDelete ) );
                _TestAnalyticsService.DeleteAllComponentByProject( await _TestAnalyticsService.GetAllComponentsByProject( projectToDelete ) );
                _TestAnalyticsService.ProjectSet().Remove( projectToDelete );
                await _TestAnalyticsService.SaveAsync();
                return Json( project );
            } catch( System.Exception ) {
                return BadRequest();
            }
        }
        [HttpPost( "save-project-attrs" )]
        public async Task<IActionResult> SaveProjectAttributes() {
            try {
                await _TestAnalyticsService.SaveAttributesAsync( Utils.GetObjectFromJsonInRequest<List<PostShared>>( Request ) );
                await _TestAnalyticsService.SaveAsync();
                return Json( await _TestAnalyticsService.GetComponentsAsync() );
            } catch( System.Exception ) {
                return BadRequest();
            }
        }
        [HttpPost( "save-project-comps" )]
        public async Task<IActionResult> SaveProjectComponents() {
            try {
                await _TestAnalyticsService.SaveComponentsAsync( Utils.GetObjectFromJsonInRequest<List<PostShared>>( Request ) );
                await _TestAnalyticsService.SaveAsync();
                return Json( await _TestAnalyticsService.GetAttributesAsync() );
            } catch( System.Exception ) {
                return BadRequest();
            }
        }
        [HttpPost( "save-project" )]
        public async Task<IActionResult> SaveProject() {
            try {
                var project = Utils.GetObjectFromJsonInRequest<ProjectModel>( Request );
                _TestAnalyticsService.ProjectSet().Update( project );
                await _TestAnalyticsService.SaveAsync();
                return Json( project );
            } catch( System.Exception ) {
                return BadRequest();
            }
        }
    }
}
