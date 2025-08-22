// Admin Expired Documents JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeExpiredDocuments();
    setupBulkActions();
});

// Admin Expired Documents JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeExpiredDocuments();
    setupBulkActions();
    setupFormHandlers();
});

function initializeExpiredDocuments() {
    console.log('Expired documents page initialized');
    updateBulkActionButtons();
}

function setupBulkActions() {
    // Setup checkbox change handlers
    const checkboxes = document.querySelectorAll('.document-checkbox');
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', updateBulkActionButtons);
    });
}

function setupFormHandlers() {
    // Setup reminder form submission
    const reminderForm = document.getElementById('reminderForm');
    if (reminderForm) {
        reminderForm.addEventListener('submit', handleReminderSubmission);
    }
}

function handleReminderSubmission(e) {
    e.preventDefault();

    const form = e.target;
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;

    // Show loading state
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang gửi...';
    submitBtn.disabled = true;

    // Get form data
    const formData = new FormData(form);
    const documentId = formData.get('DocumentId');
    const reminderMessage = formData.get('ReminderMessage');

    console.log('Sending reminder for document:', documentId);

    // Create URL-encoded form data
    const params = new URLSearchParams();
    params.append('documentId', documentId);
    params.append('reminderMessage', reminderMessage);

    // Submit form
    fetch(window.location.pathname + '?handler=SendReminder', {
        method: 'POST',
        body: params,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        }
    })
        .then(response => {
            console.log('Response status:', response.status);
            if (response.ok) {
                showAlert('Đã gửi nhắc nhở gia hạn thành công', 'success');
                const modal = bootstrap.Modal.getInstance(document.getElementById('renewalReminderModal'));
                if (modal) modal.hide();
                setTimeout(() => window.location.reload(), 1500);
            } else {
                response.text().then(text => {
                    console.error('Response error:', text);
                    showAlert('Có lỗi xảy ra khi gửi nhắc nhở', 'error');
                });
            }
        })
        .catch(error => {
            console.error('Reminder error:', error);
            showAlert('Có lỗi xảy ra khi gửi nhắc nhở', 'error');
        })
        .finally(() => {
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;
        });
}

function toggleSelectAll(selectAllCheckbox) {
    const checkboxes = document.querySelectorAll('.document-checkbox');
    checkboxes.forEach(checkbox => {
        checkbox.checked = selectAllCheckbox.checked;
    });
    updateBulkActionButtons();
}

function updateBulkActionButtons() {
    const selectedCheckboxes = document.querySelectorAll('.document-checkbox:checked');
    const selectedCount = selectedCheckboxes.length;

    // Update selected count display
    const selectedCountElement = document.getElementById('selectedCount');
    if (selectedCountElement) {
        selectedCountElement.textContent = selectedCount;
    }

    // Enable/disable bulk action buttons
    const bulkReminderBtn = document.getElementById('bulkReminderBtn');
    const bulkRenewBtn = document.getElementById('bulkRenewBtn');

    if (bulkReminderBtn) {
        bulkReminderBtn.disabled = selectedCount === 0;
    }

    if (bulkRenewBtn) {
        bulkRenewBtn.disabled = selectedCount === 0;
    }
}

function viewDocument(documentId, documentUrl, documentName) {
    try {
        console.log('Viewing document:', documentId, documentUrl, documentName);

        // Check if modal exists
        const modalElement = document.getElementById('documentViewerModal');
        if (!modalElement) {
            console.error('Document viewer modal not found');
            showAlert('Không thể mở cửa sổ xem tài liệu', 'error');
            return;
        }

        const modal = new bootstrap.Modal(modalElement);
        const viewer = document.getElementById('documentViewer');
        const modalTitle = modalElement.querySelector('.modal-title');

        if (modalTitle) {
            modalTitle.textContent = `Xem chứng chỉ: ${documentName}`;
        }

        // Show loading
        if (viewer) {
            viewer.innerHTML = '<div class="text-center p-4"><i class="fas fa-spinner fa-spin fa-2x"></i><p class="mt-2">Đang tải...</p></div>';
        }

        // Check if document URL is valid
        if (!documentUrl || documentUrl === '#') {
            if (viewer) {
                viewer.innerHTML = `
                    <div class="text-center p-4">
                        <i class="fas fa-exclamation-triangle fa-3x text-warning mb-3"></i>
                        <h5>Không có URL tài liệu</h5>
                        <p class="text-muted">Tài liệu này chưa có đường dẫn hợp lệ</p>
                    </div>
                `;
            }
            modal.show();
            return;
        }

        // Determine file type and create appropriate viewer
        const fileExtension = documentUrl.split('.').pop().toLowerCase();

        if (fileExtension === 'pdf') {
            if (viewer) {
                viewer.innerHTML = `
                    <iframe src="${documentUrl}" 
                            width="100%" 
                            height="600px" 
                            style="border: none;">
                        <p>Trình duyệt không hỗ trợ xem PDF. 
                           <a href="${documentUrl}" target="_blank">Tải xuống file</a>
                        </p>
                    </iframe>
                `;
            }
        } else if (['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension)) {
            if (viewer) {
                viewer.innerHTML = `
                    <div class="text-center">
                        <img src="${documentUrl}" 
                             class="img-fluid" 
                             style="max-height: 600px;" 
                             alt="${documentName}"
                             onload="this.style.display='block'"
                             onerror="this.style.display='none'; this.nextElementSibling.style.display='block'">
                        <div style="display: none;" class="alert alert-warning">
                            Không thể hiển thị hình ảnh. 
                            <a href="${documentUrl}" target="_blank">Tải xuống file</a>
                        </div>
                    </div>
                `;
            }
        } else {
            if (viewer) {
                viewer.innerHTML = `
                    <div class="text-center p-4">
                        <i class="fas fa-file fa-3x text-muted mb-3"></i>
                        <h5>Không thể xem trước file này</h5>
                        <p class="text-muted">Định dạng file: ${fileExtension.toUpperCase()}</p>
                        <a href="${documentUrl}" target="_blank" class="btn btn-primary">
                            <i class="fas fa-download me-2"></i>Tải xuống file
                        </a>
                    </div>
                `;
            }
        }

        modal.show();

    } catch (error) {
        console.error('Error viewing document:', error);
        showAlert('Có lỗi xảy ra khi xem tài liệu', 'error');
    }
}

function sendRenewalReminder(documentId, professionalName) {
    try {
        console.log('Sending reminder for:', documentId, professionalName);

        // Check if modal exists
        const modalElement = document.getElementById('renewalReminderModal');
        if (!modalElement) {
            console.error('Renewal reminder modal not found');
            showAlert('Không thể mở cửa sổ gửi nhắc nhở', 'error');
            return;
        }

        const modal = new bootstrap.Modal(modalElement);

        // Populate form
        const documentIdInput = document.getElementById('reminderDocumentId');
        const professionalNameInput = document.getElementById('reminderProfessionalName');

        if (documentIdInput) {
            documentIdInput.value = documentId;
        }

        if (professionalNameInput) {
            professionalNameInput.value = professionalName;
        }

        modal.show();

    } catch (error) {
        console.error('Error opening reminder modal:', error);
        showAlert('Có lỗi xảy ra khi mở form gửi nhắc nhở', 'error');
    }
}

function markAsRenewed(documentId) {
    try {
        console.log('Marking as renewed:', documentId);

        if (confirm('Bạn có chắc chắn muốn đánh dấu chứng chỉ này đã được gia hạn?')) {

            // Show loading state
            showAlert('Đang cập nhật...', 'info');

            // Create URL-encoded form data
            const params = new URLSearchParams();
            params.append('documentId', documentId);

            fetch(window.location.pathname + '?handler=MarkAsRenewed', {
                method: 'POST',
                body: params,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                }
            })
                .then(response => {
                    console.log('Mark as renewed response status:', response.status);
                    if (response.ok) {
                        showAlert('Đã đánh dấu tài liệu đã gia hạn', 'success');
                        setTimeout(() => window.location.reload(), 1500);
                    } else {
                        response.text().then(text => {
                            console.error('Mark as renewed error:', text);
                            showAlert('Có lỗi xảy ra khi cập nhật', 'error');
                        });
                    }
                })
                .catch(error => {
                    console.error('Error marking as renewed:', error);
                    showAlert('Có lỗi xảy ra khi cập nhật', 'error');
                });
        }
    } catch (error) {
        console.error('Error in markAsRenewed:', error);
        showAlert('Có lỗi xảy ra', 'error');
    }
}

function sendSelectedReminders() {
    const selectedCheckboxes = document.querySelectorAll('.document-checkbox:checked');
    const selectedIds = Array.from(selectedCheckboxes).map(cb => cb.value);

    if (selectedIds.length === 0) {
        showAlert('Vui lòng chọn ít nhất một tài liệu', 'warning');
        return;
    }

    if (confirm(`Bạn có chắc chắn muốn gửi nhắc nhở gia hạn cho ${selectedIds.length} tài liệu đã chọn?`)) {
        // Implement bulk reminder sending
        showAlert('Đang gửi nhắc nhở...', 'info');

        // Simulate API call
        setTimeout(() => {
            showAlert(`Đã gửi nhắc nhở cho ${selectedIds.length} chuyên gia y tế`, 'success');
        }, 2000);
    }
}

function markSelectedAsRenewed() {
    const selectedCheckboxes = document.querySelectorAll('.document-checkbox:checked');
    const selectedIds = Array.from(selectedCheckboxes).map(cb => cb.value);

    if (selectedIds.length === 0) {
        showAlert('Vui lòng chọn ít nhất một tài liệu', 'warning');
        return;
    }

    if (confirm(`Bạn có chắc chắn muốn đánh dấu ${selectedIds.length} tài liệu đã chọn là đã gia hạn?`)) {
        // Implement bulk renewal marking
        showAlert('Đang cập nhật...', 'info');

        // Simulate API call
        setTimeout(() => {
            showAlert(`Đã đánh dấu ${selectedIds.length} tài liệu đã được gia hạn`, 'success');
            setTimeout(() => window.location.reload(), 1500);
        }, 2000);
    }
}

function sendBulkReminders() {
    if (confirm('Bạn có chắc chắn muốn gửi nhắc nhở gia hạn cho tất cả chứng chỉ hết hạn/sắp hết hạn?')) {
        showAlert('Đang gửi nhắc nhở hàng loạt...', 'info');

        // Implement bulk reminder sending for all expired/expiring documents
        setTimeout(() => {
            showAlert('Đã gửi nhắc nhở gia hạn hàng loạt thành công', 'success');
        }, 3000);
    }
}

function refreshData() {
    showAlert('Đang làm mới dữ liệu...', 'info');
    setTimeout(() => {
        window.location.reload();
    }, 500);
}

function showAlert(message, type = 'info') {
    // Remove existing alerts
    const existingAlerts = document.querySelectorAll('.alert-auto-dismiss');
    existingAlerts.forEach(alert => alert.remove());

    // Create alert element
    const alertClasses = {
        'success': 'alert-success',
        'error': 'alert-danger',
        'warning': 'alert-warning',
        'info': 'alert-info'
    };

    const alertIcons = {
        'success': 'fas fa-check-circle',
        'error': 'fas fa-exclamation-circle',
        'warning': 'fas fa-exclamation-triangle',
        'info': 'fas fa-info-circle'
    };

    const alert = document.createElement('div');
    alert.className = `alert ${alertClasses[type] || 'alert-info'} alert-dismissible fade show alert-auto-dismiss`;
    alert.style.position = 'fixed';
    alert.style.top = '20px';
    alert.style.right = '20px';
    alert.style.zIndex = '9999';
    alert.style.minWidth = '300px';

    alert.innerHTML = `
        <i class="${alertIcons[type] || 'fas fa-info-circle'} me-2"></i>
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    document.body.appendChild(alert);

    // Auto dismiss after 5 seconds
    setTimeout(() => {
        if (alert.parentNode) {
            alert.remove();
        }
    }, 5000);
}

// Make functions globally available
window.toggleSelectAll = toggleSelectAll;
window.viewDocument = viewDocument;
window.sendRenewalReminder = sendRenewalReminder;
window.markAsRenewed = markAsRenewed;
window.sendSelectedReminders = sendSelectedReminders;
window.markSelectedAsRenewed = markSelectedAsRenewed;
window.sendBulkReminders = sendBulkReminders;
window.refreshData = refreshData;