// Líneas de producto dinámicas para los formularios de Pedido y Cotización.
// Permite agregar y quitar filas de ítems re-indexando los nombres para que el
// model binder de ASP.NET Core los reciba como una colección contigua (Items[0], Items[1], ...).
(function () {
    "use strict";

    function reindex(container) {
        const rows = container.querySelectorAll(".item-row");
        rows.forEach(function (row, index) {
            row.querySelectorAll("input, select, textarea, span, label").forEach(function (el) {
                ["name", "id", "for", "data-valmsg-for"].forEach(function (attr) {
                    const value = el.getAttribute(attr);
                    if (value) {
                        el.setAttribute(attr, value.replace(/Items(\[|_)\d+(\]|__)/, "Items$1" + index + "$2"));
                    }
                });
            });
        });
    }

    function reparseValidation(form) {
        if (window.jQuery && window.jQuery.validator && window.jQuery.validator.unobtrusive) {
            const $form = window.jQuery(form);
            $form.removeData("validator").removeData("unobtrusiveValidation");
            window.jQuery.validator.unobtrusive.parse(form);
        }
    }

    function setupItemRows(container) {
        const form = container.closest("form");
        const addButton = form.querySelector(".add-item");

        function bindRemove(row) {
            const removeButton = row.querySelector(".remove-item");
            if (!removeButton) return;
            removeButton.addEventListener("click", function () {
                if (container.querySelectorAll(".item-row").length <= 1) {
                    return; // Mantener al menos una fila.
                }
                row.remove();
                reindex(container);
            });
        }

        container.querySelectorAll(".item-row").forEach(bindRemove);

        if (addButton) {
            addButton.addEventListener("click", function () {
                const rows = container.querySelectorAll(".item-row");
                const template = rows[rows.length - 1];
                const clone = template.cloneNode(true);

                clone.querySelectorAll("input").forEach(function (input) {
                    if (input.type !== "hidden") {
                        input.value = "";
                    } else {
                        input.value = "0";
                    }
                });
                clone.querySelectorAll("select").forEach(function (select) {
                    select.selectedIndex = 0;
                });
                clone.querySelectorAll("span[data-valmsg-for]").forEach(function (span) {
                    span.textContent = "";
                });

                container.appendChild(clone);
                reindex(container);
                bindRemove(clone);
                reparseValidation(form);
            });
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll(".items-container").forEach(setupItemRows);
    });
})();
