using System.ComponentModel.DataAnnotations.Schema;

namespace CureFusion.Entities;
public enum ArticleStatus
{
    Draft,
    Published,
    Archived
}
public enum ArticleCategory
{
    Cardiology,        // أمراض القلب
    Dermatology,       // الأمراض الجلدية
    Neurology,         // طب الأعصاب
    Pediatrics,        // طب الأطفال
    Psychiatry,        // الطب النفسي
    Oncology,          // طب الأورام
    Endocrinology,     // الغدد الصماء
    Gastroenterology,  // الجهاز الهضمي
    Pulmonology,       // أمراض الرئة
    Nephrology,        // أمراض الكلى
    Ophthalmology,     // طب العيون
    Otolaryngology,    // الأنف والأذن والحنجرة (ENT)
    Rheumatology,      // أمراض الروماتيزم والمفاصل
    Obstetrics,        // طب التوليد
    Gynecology,        // طب النساء
    InfectiousDiseases,// الأمراض المعدية
    Hematology,        // أمراض الدم
    Immunology,        // علم المناعة
    Urology,           // المسالك البولية
    Orthopedics,       // جراحة العظام
    Dentistry,         // طب الأسنان
    Nutrition,         // التغذية العلاجية
    MentalHealth,      // الصحة النفسية
    GeneralMedicine,   // الطب العام
    FamilyMedicine,    // طب الأسرة
    EmergencyMedicine, // طب الطوارئ
    Anesthesiology,    // التخدير
    PublicHealth,      // الصحة العامة
    PhysicalTherapy,   // العلاج الطبيعي
    Radiology,         // الأشعة
    Pathology,         // علم الأمراض
    GeneticMedicine    // الطب الوراثي
}

public class HealthArticle
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty; 
    public string Content { get; set; } = string.Empty;
    public ArticleCategory Category { get; set; } 
    public string Tags { get; set; } = string.Empty; // Comma-separated or JSON array of tags
    public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
    public DateTime PublishedDate { get; set; } = DateTime.UtcNow; 
    public int ViewCount { get; set; } = 0;

    public string AuthorId { get; set; } = string.Empty;
   
    public ApplicationUser Author { get; set; } = null!;


    public Guid? HealthArticleImageId { get; set; }
    public UploadedFile? HealthArticleImage { get; set; } 
}
