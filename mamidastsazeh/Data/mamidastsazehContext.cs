using mamidastsazeh.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class mamidastsazehContext: IdentityDbContext
    {
        private readonly IWebHostEnvironment _environment;
        public mamidastsazehContext()
        {

        }
        public mamidastsazehContext(DbContextOptions<mamidastsazehContext> options) : base(options)
        {

        }
        public mamidastsazehContext(DbContextOptions<mamidastsazehContext> options,IWebHostEnvironment environment) : base(options)
        {
            _environment = environment;
        }
        public virtual DbSet<Category> Category { set; get; }
        public virtual DbSet<Post> Post { set; get; }
        public virtual DbSet<ContentType> ContentType { set; get; }

        public virtual DbSet<PostType> PostType { set; get; }
        public virtual DbSet<User> User { set; get; }
        public virtual DbSet<Tag> Tag { set; get; }
        public virtual DbSet<PostTag> PostTag { set; get; }
        public virtual DbSet<PostComment> PostComment { set; get; }
        public virtual DbSet<PostLike> PostLike { set; get; }
        public virtual DbSet<PostView> PostView { set; get; }
        public virtual DbSet<Media> Media { set; get; }
        public virtual DbSet<MainCategory> MainCategory { set; get; }
        public virtual DbSet<Sms> Sms { get; set; }
        public virtual DbSet<SmsToken> SmsToken { get; set; }
        public virtual DbSet<MamiEvent> MamiEvent { get; set; }
        public virtual DbSet<CommentLike> CommentLike { get; set; }
        public virtual DbSet<RestrictedWord> RestirictedWord { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Province> Province { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //به این قسمت نیازی نیست
         /*   modelBuilder.Entity<Post>()
                .Property<int>("CategoryId");
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Posts)
                .WithOne(v => v.Category)
                .HasForeignKey("CategoryId")
                .HasPrincipalKey(c=>c.Id);
                */
        }

    }
}
