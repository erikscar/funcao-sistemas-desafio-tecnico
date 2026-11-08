let beneficiariesList = [];
let editBeneficiaryIndex = null;

$(document).ready(function () {
    $("#CPF").on("input", function () {
        $(this).val(formatCpf($(this).val()));
    });

    $("#BeneficientCPF").on("input", function () {
        $(this).val(formatCpf($(this).val()));
    })

    $("#beneficiary-btn").on("click", function () {
        $("#beneficiary-modal").modal("show");
    })

    $("#addBeneficiary-btn").on("click", function () {
        const form = $("#beneficiaryForm")[0];

        if (!form.reportValidity()) return;
        var beneficiary = {
            Id: editBeneficiaryIndex != null ? beneficiariesList[editBeneficiaryIndex].Id : 0,
            Name: $("#BeneficientName").val(),
            CPF: $("#BeneficientCPF").val()
        }

        if (editBeneficiaryIndex === null) {
            beneficiariesList.push(beneficiary);
        } else {
            beneficiariesList[editBeneficiaryIndex] = beneficiary;
        }

        editBeneficiaryIndex = null;
        $("#addBeneficiary-btn").text("Incluir");

        $("#beneficiaryForm")[0].reset();
        renderBeneficiaries();
    });
});

function renderBeneficiaries() {
    const tbody = $("#beneficiariesTableBody");
    tbody.empty();

    beneficiariesList.forEach(function (beneficiary, index) {
        const row = $("<tr>");

        $("<td>").text(formatCpf(beneficiary.CPF)).appendTo(row);
        $("<td>").text(beneficiary.Name).appendTo(row);

        const actions = $("<td>");
        $("<button>", {
            type: "button",
            class: "btn btn-primary",
            text: "Alterar"
        }).css("margin-right", "8px")
            .on("click", function () {
                editBeneficiaryIndex = index;
                $("#BeneficientName").val(beneficiary.Name);
                $("#BeneficientCPF").val(formatCpf(beneficiary.CPF));
                $("#addBeneficiary-btn").text("Salvar alteração");

                renderBeneficiaries();
            }).appendTo(actions);

        $("<button>", {
            type: "button",
            class: "btn btn-primary",
            text: "Excluir",
        }).on("click", function () {
            beneficiariesList.splice(index, 1);
            editBeneficiaryIndex = null;
            $("#beneficiaryForm")[0].reset();
            $("#addBeneficiary-btn").text("Incluir");
            renderBeneficiaries();

        }).appendTo(actions);

        row.append(actions);
        tbody.append(row);
    });
}