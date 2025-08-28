// Professional Documents JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeDocumentManagement();
    setupFormValidation();
    setupTooltips();
    checkDocumentStatus();
});

function initializeDocumentManagement() {
    const uploadForm = document.getElementById('uploadForm');
    if (uploadForm) {
        uploadForm.addEventListener('submit', handleDocumentUpload);
    }

    // Setup file input change handler
    const fileInput = document.querySelector('input[name="DocumentFile"]');
    if (fileInput) {
        fileInput.addEventListener('change', handleFileSelection);
    }

    // Setup document type change handler
    const documentTypeSelect = document.querySelector('select[name="DocumentType"]');
    if (documentTypeSelect) {
        documentTypeSelect.addEventListener('change', handleDocumentTypeChange);
    }
}

function handleDocumentUpload(e) {
    e.preventDefault();
    
    const form = e.target;
    const formData = new FormData(form);
    const submitBtn = form.querySelector('button[type="submit"]');
    
    // Validate form
    if (!validateUploadForm(form)) {
        return;
    }
    
    // Show loading state
    const originalText = submitBtn.innerHTML;
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang tải lên...';
    submitBtn.disabled = true;
    
    // Submit form
    fetch(window.location.pathname, {
        method: 'POST',
        body: formData
    })
    .then(response => response.text())
    .then(html => {
        // Reload page to show updated documents
        window.location.reload();
    })
    .catch(error => {
        console.error('Upload error:', error);
        showAlert('Có lỗi xảy ra khi tải lên chứng chỉ', 'danger');
        
        // Reset button
        submitBtn.innerHTML = originalText;
        submitBtn.disabled = false;
    });
}

function handleFileSelection(e) {
    const file = e.target.files[0];
    const maxSize = 10 * 1024 * 1024; // 10MB
    const allowedTypes = ['application/pdf', 'image/jpeg', 'image/png'];
    
    if (!file) return;
    
    // Validate file size
    if (file.size > maxSize) {
        showAlert('Kích thước file không được vượt quá 10MB', 'warning');
        e.target.value = '';
        return;
    }
    
    // Validate file type
    if (!allowedTypes.includes(file.type)) {
        showAlert('Chỉ chấp nhận file PDF, JPG, PNG', 'warning');
        e.target.value = '';
        return;
    }
    
    // Update file info display
    updateFileInfo(file);
}

function handleDocumentTypeChange(e) {
    const documentType = e.target.value;
    const form = e.target.closest('form');
    
    // Update form fields based on document type
    updateFormFieldsForDocumentType(form, documentType);
    
    // Show/hide expiry date field
    const expiryField = form.querySelector('input[name="ExpiryDate"]').closest('.col-md-6');
    const requiresExpiry = ['MedicalLicense', 'SpecialtyCertificate', 'PracticeCertificate', 'ContinuingEducationCertificate'];
    
    if (requiresExpiry.includes(documentType)) {
        expiryField.style.display = '';
        form.querySelector('input[name="ExpiryDate"]').required = true;
    } else {
        expiryField.style.display = 'none';
        form.querySelector('input[name="ExpiryDate"]').required = false;
    }
    
    // Show/hide document number field
    const numberField = form.querySelector('input[name="DocumentNumber"]').closest('.col-md-6');
    const requiresNumber = ['MedicalLicense', 'IdentityDocument'];
    
    if (requiresNumber.includes(documentType)) {
        numberField.style.display = '';
        form.querySelector('input[name="DocumentNumber"]').required = true;
    } else {
        numberField.style.display = 'none';
        form.querySelector('input[name="DocumentNumber"]').required = false;
    }
}

function updateFormFieldsForDocumentType(form, documentType) {
    const nameField = form.querySelector('input[name="DocumentName"]');
    const authorityField = form.querySelector('input[name="IssuingAuthority"]');
    
    // Set default values based on document type
    const defaults = {
        'MedicalDegree': {
            name: 'Bằng tốt nghiệp Bác sĩ',
            authority: 'Bộ Giáo dục và Đào tạo'
        },
        'MedicalLicense': {
            name: 'Giấy phép hành nghề khám bệnh, chữa bệnh',
            authority: 'Bộ Y tế'
        },
        'SpecialtyCertificate': {
            name: 'Chứng chỉ chuyên khoa',
            authority: 'Bộ Y tế'
        },
        'PracticeCertificate': {
            name: 'Chứng chỉ hành nghề',
            authority: 'Sở Y tế'
        },
        'IdentityDocument': {
            name: 'Căn cước công dân',
            authority: 'Công an nhân dân'
        },
        'ContinuingEducationCertificate': {
            name: 'Chứng chỉ đào tạo liên tục',
            authority: 'Bộ Y tế'
        }
    };
    
    if (defaults[documentType]) {
        if (!nameField.value) {
            nameField.value = defaults[documentType].name;
        }
        if (!authorityField.value) {
            authorityField.value = defaults[documentType].authority;
        }
    }
}

function updateFileInfo(file) {
    const fileSize = formatFileSize(file.size);
    const fileName = file.name;
    
    // Create or update file info display
    let fileInfo = document.querySelector('.file-info');
    if (!fileInfo) {
        fileInfo = document.createElement('div');
        fileInfo.className = 'file-info mt-2 p-2 bg-light rounded';
        document.querySelector('input[name="DocumentFile"]').parentNode.appendChild(fileInfo);
    }
    
    fileInfo.innerHTML = `
        <small class="text-muted">
            <i class="fas fa-file me-1"></i>
            ${fileName} (${fileSize})
        </small>
    `;
}

function validateUploadForm(form) {
    const requiredFields = form.querySelectorAll('[required]');
    let isValid = true;
    
    requiredFields.forEach(field => {
        if (!field.value.trim()) {
            field.classList.add('is-invalid');
            isValid = false;
        } else {
            field.classList.remove('is-invalid');
        }
    });
    
    if (!isValid) {
        showAlert('Vui lòng điền đầy đủ thông tin bắt buộc', 'warning');
    }
    
    return isValid;
}

function setupFormValidation() {
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        const inputs = form.querySelectorAll('input[required], select[required]');
        inputs.forEach(input => {
            input.addEventListener('blur', function() {
                if (this.value.trim()) {
                    this.classList.remove('is-invalid');
                    this.classList.add('is-valid');
                } else {
                    this.classList.remove('is-valid');
                    this.classList.add('is-invalid');
                }
            });
        });
    });
}

function setupTooltips() {
    // Initialize Bootstrap tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

function checkDocumentStatus() {
    // Auto-refresh document status every 30 seconds
    setInterval(function() {
        const documentCards = document.querySelectorAll('.document-card[data-document-id]');
        documentCards.forEach(card => {
            const documentId = card.getAttribute('data-document-id');
            if (documentId) {
                checkSingleDocumentStatus(documentId);
            }
        });
    }, 30000); // 30 seconds
}

function checkSingleDocumentStatus(documentId) {
    fetch(`?handler=DocumentStatus&documentId=${documentId}`)
        .then(response => response.json())
        .then(data => {
            if (data.success !== false) {
                updateDocumentStatusDisplay(documentId, data);
            }
        })
        .catch(error => {
            console.error('Status check error:', error);
        });
}

function updateDocumentStatusDisplay(documentId, statusData) {
    const card = document.querySelector(`.document-card[data-document-id="${documentId}"]`);
    if (!card) return;
    
    const statusElement = card.querySelector('.document-status');
    if (statusElement) {
        statusElement.textContent = statusData.statusName;
        statusElement.className = `document-status status-${statusData.status.toLowerCase()}`;
    }
    
    // Update admin notes if present
    if (statusData.adminNotes) {
        updateAdminNotes(card, statusData.adminNotes);
    }
    
    // Update rejection reason if present
    if (statusData.rejectionReason) {
        updateRejectionReason(card, statusData.rejectionReason);
    }
}

function updateAdminNotes(card, notes) {
    let notesElement = card.querySelector('.admin-notes');
    if (!notesElement && notes) {
        notesElement = document.createElement('div');
        notesElement.className = 'admin-notes';
        card.appendChild(notesElement);
    }
    
    if (notesElement) {
        notesElement.innerHTML = `
            <small class="text-muted">
                <i class="fas fa-comment me-1"></i>
                <strong>Ghi chú:</strong> ${notes}
            </small>
        `;
    }
}

function updateRejectionReason(card, reason) {
    let reasonElement = card.querySelector('.rejection-reason');
    if (!reasonElement && reason) {
        reasonElement = document.createElement('div');
        reasonElement.className = 'rejection-reason';
        card.appendChild(reasonElement);
    }
    
    if (reasonElement) {
        reasonElement.innerHTML = `
            <small class="text-danger">
                <i class="fas fa-info-circle me-1"></i>
                <strong>Lý do từ chối:</strong> ${reason}
            </small>
        `;
    }
}

function replaceDocument(documentId, documentType) {
    // Fill upload modal with existing document info for replacement
    const modal = document.getElementById('uploadModal');
    const form = modal.querySelector('form');
    
    // Set document type
    form.querySelector('select[name="DocumentType"]').value = documentType;
    
    // Trigger change event to update form fields
    form.querySelector('select[name="DocumentType"]').dispatchEvent(new Event('change'));
    
    // Show modal
    const bootstrapModal = new bootstrap.Modal(modal);
    bootstrapModal.show();
}

function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

function showAlert(message, type = 'info') {
    // Create alert element
    const alert = document.createElement('div');
    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    // Insert at top of container
    const container = document.querySelector('.container-fluid');
    container.insertBefore(alert, container.firstChild);
    
    // Auto dismiss after 5 seconds
    setTimeout(() => {
        alert.remove();
    }, 5000);
}

// Global functions for button actions
window.replaceDocument = replaceDocument;
