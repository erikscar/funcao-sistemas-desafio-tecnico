let beneficiarios = [];
let editarBeneficiarioIndice = null;

$(document).ready(function () {
    $("#CPF").on("input", function () {
        $(this).val(formatarCPF($(this).val()));
    });

    $("#BeneficiarioCPF").on("input", function () {
        $(this).val(formatarCPF($(this).val()));
    })

    $("#beneficiario-btn").on("click", function () {
        $("#beneficiario-modal").modal("show");
    })

    $("#adicionarBeneficiario-btn").on("click", function () {
        const form = $("#beneficiarioForm")[0];

        if (!form.reportValidity()) return;
        var beneficiario = {
            Id: editarBeneficiarioIndice != null ? beneficiarios[editarBeneficiarioIndice].Id : 0,
            Nome: $("#BeneficiarioNome").val(),
            CPF: $("#BeneficiarioCPF").val()
        }

        if (editarBeneficiarioIndice === null) {
            beneficiarios.push(beneficiario);
        } else {
            beneficiarios[editarBeneficiarioIndice] = beneficiario;
        }

        editarBeneficiarioIndice = null;
        $("#adicionarBeneficiario-btn").text("Incluir");

        $("#beneficiarioForm")[0].reset();
        ListarBeneficiarios();
    });
});

function ListarBeneficiarios() {
    const tbody = $("#beneficiariosTableBody");
    tbody.empty();

    beneficiarios.forEach(function (beneficiario, index) {
        const row = $("<tr>");

        $("<td>").text(formatarCPF(beneficiario.CPF)).appendTo(row);
        $("<td>").text(beneficiario.Nome).appendTo(row);

        const actions = $("<td>");
        $("<button>", {
            type: "button",
            class: "btn btn-primary",
            text: "Alterar"
        }).css("margin-right", "8px")
            .on("click", function () {
                editarBeneficiarioIndice = index;
                $("#BeneficiarioNome").val(beneficiario.Nome);
                $("#BeneficiarioCPF").val(formatarCPF(beneficiario.CPF));
                $("#adicionarBeneficiario-btn").text("Salvar alteração");

                ListarBeneficiarios();
            }).appendTo(actions);

        $("<button>", {
            type: "button",
            class: "btn btn-primary",
            text: "Excluir",
        }).on("click", function () {
            beneficiarios.splice(index, 1);
            editarBeneficiarioIndice = null;
            $("#beneficiarioForm")[0].reset();
            $("#adicionarBeneficiario-btn").text("Incluir");
            ListarBeneficiarios();

        }).appendTo(actions);

        row.append(actions);
        tbody.append(row);
    });
}