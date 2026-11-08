
$(document).ready(function () {
    let beneficiariesList = []; 
    let editBeneficiaryIndex = null;
    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
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
                "Telefone": $(this).find("#Telefone").val()
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                }
        });
    })

    $("#beneficiary-btn").on("click", function () {
        $("#beneficiary-modal").modal("show");
    })

    $("#addBeneficiary-btn").on("click", function () {
        const form = $("#beneficiaryForm")[0];
        if (!form.reportValidity()) return;
        var beneficiary = {
            Nome: $("#BeneficientName").val(),
            CPF: $("#BeneficientCPF").val()
        }

        if (editBeneficiaryIndex === null) {
            beneficiariesList.push(beneficiary);
        } else {
            beneficiariesList[editBeneficiaryIndex] = beneficiary;
        }

        editingIndex = null;
        $("#addBeneficiary-btn").text("Incluir");
        
        form.reset();
        renderBeneficiaries();
    $("#CPF").on("input", function () {
        //Format for CPF
        var cpf = $(this).val();

        cpf = cpf.replace(/\D/g, "").slice(0, 11);
        cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
        cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
        cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

        $(this).val(cpf);
    });
})

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

function renderBeneficiaries() {
    const tbody = $("#beneficiariesTableBody");
    tbody.empty();

    beneficiariesList.forEach(function (beneficiary, index) {
        const row = $("<tr>");

        $("<td>").text(beneficiary.CPF).appendTo(row);
        $("<td>").text(beneficiary.Nome).appendTo(row);

        const actions = $("<td>");
        $("<button>", {
            type: "button",
            class: "btn btn-primary",
            text: "Alterar"
        }).on("click", function () {
            editBeneficiaryIndex = index;
            $("#BeneficientName").val(beneficiary.Nome);
            $("#BeneficientCPF").val(beneficiary.CPF);
            $("#addBeneficiary-btn").text("Salvar alteração");

            renderBeneficiaries();
        }).appendTo(actions);

        $("<button>", {
            type: "button",
            class: "btn btn-primary",
            text: "Excluir"
        }).on("click", function () {
            beneficiariesList.splice(index, 1);
            renderBeneficiaries();
        }).appendTo(actions);

        row.append(actions);
        tbody.append(row);
    });
}})
