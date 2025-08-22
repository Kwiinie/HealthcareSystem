// Admin Document Verification JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeDocumentVerification();
    setupEventListeners();
    setupModals();
});

function initializeDocumentVerification() {
    // Initialize any required components
    console.log('Document verification page initialized');

    // Auto-refresh statistics every 5 minutes
    setInterval(refreshStatistics, 300000);
}

function setupEventListeners() {
    // Setup form submissions
    const verifyForm = document.getElementById('verifyForm');
    if (verifyForm) {
        verifyForm.addEventListener('submit', handleVerifySubmission);
    }

    const rejectForm = document.getElementById('rejectForm');
    if (rejectForm) {
        rejectForm.addEventListener('submit', handleRejectSubmission);
    }

    // Setup filter form
    const filterForm = document.querySelector('form[method="get"]');
    if (filterForm) {
        const filterInputs = filterForm.querySelectorAll('select, input');
        filterInputs.forEach(input => {
            input.addEventListener('change', debounce(handleFilterChange, 300));
        });
    }
}

function setupModals() {
    // Initialize Bootstrap modals
    const verifyModal = document.getElementById('verifyModal');
    const rejectModal = document.getElementById('rejectModal');
    const documentViewerModal = document.getElementById('documentViewerModal');

    if (verifyModal) {
        verifyModal.addEventListener('hidden.bs.modal', function () {
            document.getElementById('verifyForm').reset();
        });
    }

    if (rejectModal) {
        rejectModal.addEventListener('hidden.bs.modal', function () {
            document.getElementById('rejectForm').reset();
        });
    }

    if (documentViewerModal) {
        documentViewerModal.addEventListener('hidden.bs.modal', function () {
            document.getElementById('documentViewer').innerHTML = '';
        });
    }
}

function viewDocument(documentId, documentUrl, documentName) {
    try {
        const modal = new bootstrap.Modal(document.getElementById('documentViewerModal'));
        const viewer = document.getElementById('documentViewer');
        const modalTitle = document.querySelector('#documentViewerModal .modal-title');

        modalTitle.textContent = `Xem chứng chỉ: ${documentName}`;

        // Show loading
        viewer.innerHTML = '<div class="text-center p-4"><i class="fas fa-spinner fa-spin fa-2x"></i><p class="mt-2">Đang tải...</p></div>';

        // Determine file type and create appropriate viewer
        const fileExtension = documentUrl.split('.').pop().toLowerCase();

        if (fileExtension === 'pdf') {
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
        } else if (['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension)) {
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
        } else {
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

        modal.show();

    } catch (error) {
        console.error('Error viewing document:', error);
        showAlert('Có lỗi xảy ra khi xem tài liệu', 'error');
    }
}

function verifyDocument(documentId, professionalName, documentName) {
    try {
        const modal = new bootstrap.Modal(document.getElementById('verifyModal'));

        // Populate form
        document.getElementById('verifyDocumentId').value = documentId;
        document.getElementById('verifyProfessionalName').value = professionalName;
        document.getElementById('verifyDocumentName').value = documentName;

        modal.show();

    } catch (error) {
        console.error('Error opening verify modal:', error);
        showAlert('Có lỗi xảy ra', 'error');
    }
}

function rejectDocument(documentId, professionalName, documentName) {
    try {
        const modal = new bootstrap.Modal(document.getElementById('rejectModal'));

        // Populate form
        document.getElementById('rejectDocumentId').value = documentId;
        document.getElementById('rejectProfessionalName').value = professionalName;
        document.getElementById('rejectDocumentName').value = documentName;

        modal.show();

    } catch (error) {
        console.error('Error opening reject modal:', error);
        showAlert('Có lỗi xảy ra', 'error');
    }
}

function handleVerifySubmission(e) {
    e.preventDefault();

    const form = e.target;
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;

    // Show loading state
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...';
    submitBtn.disabled = true;

    // Get form data
    const formData = new FormData(form);
    const documentId = formData.get('DocumentId');
    const adminNotes = formData.get('AdminNotes');

    console.log('Submitting verification for document:', documentId);

    // Create URL-encoded form data for proper ASP.NET Core handling
    const params = new URLSearchParams();
    params.append('documentId', documentId);
    params.append('verificationStatus', 'Verified');
    if (adminNotes) {
        params.append('adminNotes', adminNotes);
    }

    // Submit form with proper handler
    fetch(window.location.pathname + '?handler=VerifyDocument', {
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
                showAlert('Xác thực chứng chỉ thành công', 'success');
                bootstrap.Modal.getInstance(document.getElementById('verifyModal')).hide();
                setTimeout(() => window.location.reload(), 1500);
            } else {
                response.text().then(text => {
                    console.error('Response error:', text);
                    showAlert('Có lỗi xảy ra khi xác thực', 'error');
                });
            }
        })
        .catch(error => {
            console.error('Verify error:', error);
            showAlert('Có lỗi xảy ra khi xác thực', 'error');
        })
        .finally(() => {
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;
        });
}

function handleRejectSubmission(e) {
    e.preventDefault();

    const form = e.target;
    const rejectionReason = form.querySelector('textarea[name="RejectionReason"]').value.trim();

    if (!rejectionReason) {
        showAlert('Vui lòng nhập lý do từ chối', 'warning');
        return;
    }

    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;

    // Show loading state
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...';
    submitBtn.disabled = true;

    // Get form data
    const formData = new FormData(form);
    const documentId = formData.get('DocumentId');
    const adminNotes = formData.get('AdminNotes');

    console.log('Submitting rejection for document:', documentId);

    // Create URL-encoded form data for proper ASP.NET Core handling
    const params = new URLSearchParams();
    params.append('documentId', documentId);
    params.append('rejectionReason', rejectionReason);
    if (adminNotes) {
        params.append('adminNotes', adminNotes);
    }

    // Submit form with proper handler
    fetch(window.location.pathname + '?handler=RejectDocument', {
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
                showAlert('Từ chối chứng chỉ thành công', 'success');
                bootstrap.Modal.getInstance(document.getElementById('rejectModal')).hide();
                setTimeout(() => window.location.reload(), 1500);
            } else {
                response.text().then(text => {
                    console.error('Response error:', text);
                    showAlert('Có lỗi xảy ra khi từ chối', 'error');
                });
            }
        })
        .catch(error => {
            console.error('Reject error:', error);
            showAlert('Có lỗi xảy ra khi từ chối', 'error');
        })
        .finally(() => {
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;
        });
}

function handleRejectionReasonChange(selectElement) {
    const reasonTextarea = selectElement.parentNode.querySelector('textarea[name="RejectionReason"]');
    const selectedValue = selectElement.value;

    const predefinedReasons = {
        'invalid_document': 'Chứng chỉ không hợp lệ hoặc không đúng định dạng quy định.',
        'expired': 'Chứng chỉ đã hết hạn sử dụng.',
        'poor_quality': 'Chất lượng hình ảnh/tài liệu quá kém, không thể xác thực.',
        'missing_info': 'Thiếu thông tin bắt buộc như số hiệu, ngày cấp, cơ quan cấp.',
        'fake_document': 'Có dấu hiệu nghi ngờ tài liệu giả mạo.',
        'wrong_type': 'Sai loại chứng chỉ so với yêu cầu.'
    };

    if (predefinedReasons[selectedValue]) {
        reasonTextarea.value = predefinedReasons[selectedValue];
    } else if (selectedValue === 'other') {
        reasonTextarea.value = '';
        reasonTextarea.placeholder = 'Nhập lý do cụ thể...';
    }
}

function handleFilterChange() {
    const form = document.querySelector('form[method="get"]');
    if (form) {
        form.submit();
    }
}

function refreshData() {
    showAlert('Đang làm mới dữ liệu...', 'info');
    setTimeout(() => {
        window.location.reload();
    }, 500);
}

function showExpiringDocuments() {
    fetch('?handler=ExpiringDocuments&daysAhead=30')
        .then(response => response.json())
        .then(data => {
            if (data.success && data.data.length > 0) {
                displayExpiringDocuments(data.data);
            } else {
                showAlert('Không có chứng chỉ nào sắp hết hạn trong 30 ngày tới', 'info');
            }
        })
        .catch(error => {
            console.error('Error fetching expiring documents:', error);
            showAlert('Có lỗi xảy ra khi tải dữ liệu', 'error');
        });
}

function displayExpiringDocuments(documents) {
    const modalHtml = `
        <div class="modal fade" id="expiringDocumentsModal" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <i class="fas fa-exclamation-triangle text-warning me-2"></i>
                            Chứng chỉ sắp hết hạn
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <table class="table table-sm">
                                <thead>
                                    <tr>
                                        <th>Bác sĩ</th>
                                        <th>Chứng chỉ</th>
                                        <th>Loại</th>
                                        <th>Hết hạn</th>
                                        <th>Còn lại</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    ${documents.map(doc => `
                                        <tr class="${doc.daysUntilExpiry <= 7 ? 'table-danger' : doc.daysUntilExpiry <= 30 ? 'table-warning' : ''}">
                                            <td>${doc.professionalName}</td>
                                            <td>${doc.documentName}</td>
                                            <td>${doc.documentTypeName}</td>
                                            <td>${doc.expiryDate}</td>
                                            <td>
                                                <span class="badge ${doc.daysUntilExpiry <= 7 ? 'bg-danger' : 'bg-warning'}">
                                                    ${doc.daysUntilExpiry} ngày
                                                </span>
                                            </td>
                                        </tr>
                                    `).join('')}
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    // Remove existing modal if any
    const existingModal = document.getElementById('expiringDocumentsModal');
    if (existingModal) {
        existingModal.remove();
    }

    // Add modal to body
    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('expiringDocumentsModal'));
    modal.show();

    // Remove modal from DOM when hidden
    document.getElementById('expiringDocumentsModal').addEventListener('hidden.bs.modal', function () {
        this.remove();
    });
}

function refreshStatistics() {
    // Refresh statistics without full page reload
    fetch(window.location.pathname + '?partial=statistics')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                updateStatisticsDisplay(data.statistics);
            }
        })
        .catch(error => {
            console.error('Error refreshing statistics:', error);
        });
}

function updateStatisticsDisplay(statistics) {
    const statCards = document.querySelectorAll('.stat-card');

    statCards.forEach(card => {
        if (card.classList.contains('pending')) {
            card.querySelector('h3').textContent = statistics.pendingVerification;
        } else if (card.classList.contains('under-review')) {
            card.querySelector('h3').textContent = statistics.underReview;
        } else if (card.classList.contains('verified')) {
            card.querySelector('h3').textContent = statistics.verified;
        } else if (card.classList.contains('rejected')) {
            card.querySelector('h3').textContent = statistics.rejected;
        }
    });
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

// Utility function for debouncing
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function setUnderReview(documentId, professionalName, documentName) {
    try {
        const modal = new bootstrap.Modal(document.getElementById('underReviewModal'));

        // Populate form
        document.getElementById('underReviewDocumentId').value = documentId;
        document.getElementById('underReviewProfessionalName').value = professionalName;
        document.getElementById('underReviewDocumentName').value = documentName;

        modal.show();

    } catch (error) {
        console.error('Error opening under review modal:', error);
        showAlert('Có lỗi xảy ra', 'error');
    }
}

function viewDocumentDetails(documentId) {
    try {
        const modal = new bootstrap.Modal(document.getElementById('documentDetailsModal'));
        const content = document.getElementById('documentDetailsContent');

        // Show loading
        content.innerHTML = '<div class="text-center p-4"><i class="fas fa-spinner fa-spin fa-2x"></i><p class="mt-2">Đang tải...</p></div>';
        modal.show();

        // Fetch document details
        fetch(`?handler=DocumentDetails&documentId=${documentId}`)
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    const doc = data.data;
                    content.innerHTML = `
                        <div class="row">
                            <div class="col-md-6">
                                <h6 class="fw-bold">Thông tin chuyên gia</h6>
                                <p><strong>Họ tên:</strong> ${doc.professionalName || 'N/A'}</p>
                                <p><strong>ID:</strong> #${doc.professionalId || 'N/A'}</p>
                            </div>
                            <div class="col-md-6">
                                <h6 class="fw-bold">Thông tin chứng chỉ</h6>
                                <p><strong>Loại:</strong> ${doc.documentTypeName || 'N/A'}</p>
                                <p><strong>Tên:</strong> ${doc.documentName || 'N/A'}</p>
                                <p><strong>Số hiệu:</strong> ${doc.documentNumber || 'Chưa có'}</p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <h6 class="fw-bold">Thông tin cấp phát</h6>
                                <p><strong>Ngày cấp:</strong> ${doc.issueDate || 'Chưa có'}</p>
                                <p><strong>Ngày hết hạn:</strong> ${doc.expiryDate || 'Không thời hạn'}</p>
                                <p><strong>Cơ quan cấp:</strong> ${doc.issuingAuthority || 'Chưa có'}</p>
                            </div>
                            <div class="col-md-6">
                                <h6 class="fw-bold">Trạng thái xác thực</h6>
                                <p><strong>Trạng thái:</strong> <span class="badge bg-${getStatusColor(doc.verificationStatus)}">${doc.verificationStatusName}</span></p>
                                <p><strong>Ngày tải lên:</strong> ${doc.createdAt || 'N/A'}</p>
                                ${doc.adminNotes ? `<p><strong>Ghi chú admin:</strong> ${doc.adminNotes}</p>` : ''}
                                ${doc.rejectionReason ? `<p><strong>Lý do từ chối:</strong> <span class="text-danger">${doc.rejectionReason}</span></p>` : ''}
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-12">
                                <h6 class="fw-bold">Hành động</h6>
                                <button class="btn btn-primary me-2" onclick="viewDocument(${doc.id}, '${doc.documentUrl}', '${doc.documentName}')">
                                    <i class="fas fa-eye me-1"></i>Xem tài liệu
                                </button>
                                <a href="${doc.documentUrl}" target="_blank" class="btn btn-outline-primary">
                                    <i class="fas fa-download me-1"></i>Tải xuống
                                </a>
                            </div>
                        </div>
                    `;
                } else {
                    content.innerHTML = '<div class="alert alert-danger">Không thể tải chi tiết tài liệu</div>';
                }
            })
            .catch(error => {
                console.error('Error fetching document details:', error);
                content.innerHTML = '<div class="alert alert-danger">Có lỗi xảy ra khi tải chi tiết</div>';
            });

    } catch (error) {
        console.error('Error viewing document details:', error);
        showAlert('Có lỗi xảy ra', 'error');
    }
}

function getStatusColor(status) {
    switch (status?.toLowerCase()) {
        case 'pendingverification': return 'warning';
        case 'underreview': return 'info';
        case 'verified': return 'success';
        case 'rejected': return 'danger';
        default: return 'secondary';
    }
}

// Add this function to handle under review form submission
function setupUnderReviewForm() {
    const underReviewForm = document.getElementById('underReviewForm');
    if (underReviewForm) {
        underReviewForm.addEventListener('submit', handleUnderReviewSubmission);
    }
}

function handleUnderReviewSubmission(e) {
    e.preventDefault();

    const form = e.target;
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;

    // Show loading state
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...';
    submitBtn.disabled = true;

    // Get form data
    const formData = new FormData(form);
    const documentId = formData.get('DocumentId');
    const adminNotes = formData.get('AdminNotes');

    console.log('Setting under review for document:', documentId);

    // Create URL-encoded form data
    const params = new URLSearchParams();
    params.append('documentId', documentId);
    params.append('verificationStatus', 'UnderReview');
    if (adminNotes) {
        params.append('adminNotes', adminNotes);
    }

    // Submit form with proper handler
    fetch(window.location.pathname + '?handler=SetUnderReview', {
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
                showAlert('Đã đặt trạng thái đang xem xét thành công', 'success');
                bootstrap.Modal.getInstance(document.getElementById('underReviewModal')).hide();
                setTimeout(() => window.location.reload(), 1500);
            } else {
                response.text().then(text => {
                    console.error('Response error:', text);
                    showAlert('Có lỗi xảy ra khi đặt trạng thái', 'error');
                });
            }
        })
        .catch(error => {
            console.error('Under review error:', error);
            showAlert('Có lỗi xảy ra khi đặt trạng thái', 'error');
        })
        .finally(() => {
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;
        });
}

// Update the initialization function
function initializeDocumentVerification() {
    // Initialize any required components
    console.log('Document verification page initialized');

    // Setup under review form
    setupUnderReviewForm();

    // Auto-refresh statistics every 5 minutes
    setInterval(refreshStatistics, 300000);
}

// Make functions globally available
window.setUnderReview = setUnderReview;
window.viewDocumentDetails = viewDocumentDetails;