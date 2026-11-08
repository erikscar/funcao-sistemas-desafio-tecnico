
$(document).ready(function () {
    let beneficiariesList = [];
    let editBeneficiaryIndex = null;

    $("#CPF").on("input", function () {
        $(this).val(formatCpf($(this).val()));
    });

    $("#BeneficientCPF").on("input", function () {
        $(this).val(formatCpf($(this).val()));
    })

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPostClient,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "CPF": $(this).find("#CPF").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "Beneficiarios": beneficiariesList
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success: function (r) {
                ModalDialog("Sucesso!", r.message)

                $("#formCadastro")[0].reset();
                $("#beneficiaryForm")[0].reset();

                $("#addBeneficiary-btn").text("Incluir");
                renderBeneficiaries();
            }
        });
    });

    $("#beneficiary-btn").on("click", function () {
        $("#beneficiary-modal").modal("show");
    })

    $("#addBeneficiary-btn").on("click", function () {
        const form = $("#beneficiaryForm")[0];
        if (!form.reportValidity()) return;
        var beneficiary = {
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
});

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
