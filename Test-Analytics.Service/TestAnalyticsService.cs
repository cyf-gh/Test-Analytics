using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Test_Analytics.Model;
using Test_Analytics.Service.Database;

namespace Test_Analytics.Service {
    public interface ITestAnalyticsService {

        Task<TestAnalyticsViewModel> GetViewModelByProject( ProjectModel project );

        Task<List<AttributeModel>> GetAllAttributesByProject( ProjectModel project );
        Task<List<ComponentModel>> GetAllComponentsByProject( ProjectModel project );
        Task<List<CapabilityModel>> GetAllCapabilitiesByProject( ProjectModel project );

        DbSet<ComponentModel> ComponentSet();
        DbSet<AttributeModel> AttributeSet();
        DbSet<CapabilityModel> CapabilitySet();
        DbSet<ProjectModel> ProjectSet();

        Task<List<AttributeModel>> GetAttributesAsync();
        Task<List<ComponentModel>> GetComponentsAsync();
        Task<List<CapabilityModel>> GetCapabilitiesAsync();
        Task<List<ProjectModel>> GetProjectsAsync();

        Task<bool> SaveAsync();
        Task SaveAttributesAsync( List<PostShared> list );
        Task SaveComponentsAsync( List<PostShared> list );
        Task SaveCapabilitiesAsync( List<PostCapa> list );
        void DeleteAllCapabilityByProject( List<CapabilityModel> capabilities );
        void DeleteAllAttributesByProject( List<AttributeModel> list );
        void DeleteAllComponentByProject( List<ComponentModel> list );
    }
    public class PostShared {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Tags { get; set; }
        public bool IsSuffi { get; set; }
    }
    public class PostCapa {
        public int ProjectId { get; set; }
        public string Tags { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public FrequencyOfFailure FoF { get; set; }
        public Impact Impact { get; set; }
        public string Attr { get; set; }
        public string Comp { get; set; }
        static public List<CapabilityModel> ToModel(
            List<PostCapa> postCapas,
            List<AttributeModel> attributes,
            List<ComponentModel> components,
            ProjectModel project ) {
            List<CapabilityModel> capas = new List<CapabilityModel>();
            if( postCapas.Count == 0 ) {
                return null;
            }
            foreach( var postCapa in postCapas ) {
                CapabilityModel capa = new CapabilityModel();
                capa.Project = project;
                capa.Tags = postCapa.Tags;
                capa.Name = postCapa.Name;
                capa.Description = postCapa.Desc;
                capa.FrequencyOfFailure = postCapa.FoF;
                capa.Impact = postCapa.Impact;
                capa.Attribute = attributes.Find( a => { return a.Name == postCapa.Attr; } );
                capa.Component = components.Find( c => { return c.Name == postCapa.Comp; } );
                capas.Add( capa );
            }
            return capas;
        }
    }
    public static class PostToModel<T> where T : _SharedModel, new() {
        public static List<T> ToSharedModel( List<PostShared> postShareds, ProjectModel project ) {
            List<T> shares = new List<T>();
            if( postShareds.Count == 0 ) {
                return null;
            }
            foreach( var postShared in postShareds ) {
                T share = new T();
                share.Project = project;
                share.Tags = postShared.Tags;
                share.Description = postShared.Desc;
                share.ValidOperation = postShared.IsSuffi;
                share.Name = postShared.Name;
                shares.Add( share as T );
            }
            return shares;
        }
    }

    public class TestAnalyticsServiceImpl : ITestAnalyticsService {
        private readonly ACCContext _Context;
        public async Task SaveCapabilitiesAsync( List<PostCapa> postCapas ) {
            if( postCapas.Count == 0 ) {
                return;
            }
            ProjectModel project = ( await GetProjectsAsync() ).Find( p => { return p.Id == postCapas[0].ProjectId; } );
            DeleteAllCapabilityByProject( await GetAllCapabilitiesByProject( project ) );
            var capas = PostCapa.ToModel(
                postCapas,
                await GetAllAttributesByProject( project ),
                await GetAllComponentsByProject( project ),
                project );
            await CapabilitySet().AddRangeAsync( capas );
        }
        public async Task SaveAttributesAsync( List<PostShared> postAttrs ) {
            if( postAttrs.Count == 0 ) {
                return;
            }
            ProjectModel project = ( await GetProjectsAsync() ).Find( p => { return p.Id == postAttrs[0].ProjectId; } );
            // Prevent Sqlite: FOREIGN KEY constraint failed on remove
            DeleteAllCapabilityByProject( await GetAllCapabilitiesByProject( project ) );
            // Delete all old attributes
            DeleteAllAttributesByProject( await GetAllAttributesByProject( project ) );
            // Update all new attributes
            var attrs = PostToModel<AttributeModel>.ToSharedModel( postAttrs, project );
            AttributeSet().UpdateRange( attrs );
        }
        public async Task SaveComponentsAsync( List<PostShared> postComps ) {
            if( postComps.Count == 0 ) {
                return;
            }
            ProjectModel project = ( await GetProjectsAsync() ).Find( p => { return p.Id == postComps[0].ProjectId; } );
            // Prevent Sqlite: FOREIGN KEY constraint failed on remove
            DeleteAllCapabilityByProject( await GetAllCapabilitiesByProject( project ) );
            // Delete all old components
            DeleteAllComponentByProject( await GetAllComponentsByProject( project ) );
            // Update all new components
            var compos = PostToModel<ComponentModel>.ToSharedModel( postComps, project );
            ComponentSet().UpdateRange( compos );
        }
        public TestAnalyticsServiceImpl( ACCContext context ) {
            _Context = context;
        }
        public async Task<TestAnalyticsViewModel> GetViewModelByProject( ProjectModel project ) {
            TestAnalyticsViewModel viewModel = new TestAnalyticsViewModel();

            viewModel.Project = project;
            viewModel.Attributes = await GetAllAttributesByProject( project );
            viewModel.Components = await GetAllComponentsByProject( project );
            viewModel.Capabilities = await GetAllCapabilitiesByProject( project );
            viewModel.Projects = await GetProjectsAsync();
            return viewModel;
        }

        public void DeleteAllAttributesByProject( List<AttributeModel> attributes ) {
            if( attributes.Count == 0 ) {
                return;
            }
            AttributeSet().RemoveRange( attributes );
        }
        public async Task<List<AttributeModel>> GetAllAttributesByProject( ProjectModel project ) {
            return ( await GetAttributesAsync() ).FindAll( a => { return a.Project == project; } );
        }
        public void DeleteAllComponentByProject( List<ComponentModel> components ) {
            if( components.Count == 0 ) {
                return;
            }
            ComponentSet().RemoveRange( components );
        }
        public async Task<List<ComponentModel>> GetAllComponentsByProject( ProjectModel project ) {
            return ( await GetComponentsAsync() ).FindAll( a => { return a.Project == project; } );
        }
        public void DeleteAllCapabilityByProject( List<CapabilityModel> capabilities ) {
            if( capabilities.Count == 0 ) {
                return;
            }
            CapabilitySet().RemoveRange( capabilities );
        }
        public async Task<List<CapabilityModel>> GetAllCapabilitiesByProject( ProjectModel project ) {
            return ( await GetCapabilitiesAsync() ).FindAll( a => { return a.Project == project; } );
        }

        public DbSet<AttributeModel> AttributeSet() {
            return _Context.AttributeModels;
        }
        public DbSet<ComponentModel> ComponentSet() {
            return _Context.ComponentModels;
        }
        public DbSet<CapabilityModel> CapabilitySet() {
            return _Context.CapabilityModels;
        }

        public DbSet<ProjectModel> ProjectSet() {
            return _Context.ProjectModels;
        }

        public async Task<List<AttributeModel>> GetAttributesAsync() {
            return await _Context.AttributeModels.ToListAsync();
        }
        public async Task<List<ComponentModel>> GetComponentsAsync() {
            return await _Context.ComponentModels.ToListAsync();
        }
        public async Task<List<CapabilityModel>> GetCapabilitiesAsync() {
            return await _Context.CapabilityModels.ToListAsync();
        }
        public async Task<List<ProjectModel>> GetProjectsAsync() {
            return await ProjectSet().ToListAsync();
        }

        public async Task<bool> SaveAsync() {
            return await _Context.SaveChangesAsync() > 0;
        }

    }
}
