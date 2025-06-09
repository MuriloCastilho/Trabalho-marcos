using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Entities;
using WebApplication4.Utils.Enums;

public class Venda
{
    [Required]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string NomeProduto {  get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Data { get; set; }

    [Required]
    public TipoPagamentoEnum TipoPagamento { get; set; }

    public float ValorTotal { get; set; }

    [ForeignKey("Cliente")]
    public long ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    [ForeignKey("Medicamento")]
    public long MedicamentoId { get; set; }

    public Medicamento Medicamento { get; set; } = null!;

    [ForeignKey("Funcionario")]
    public long FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; } = null!;
}
