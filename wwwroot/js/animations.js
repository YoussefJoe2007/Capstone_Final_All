// ===== ANIMATIONS JAVASCRIPT =====

// Wait for DOM to be ready
document.addEventListener('DOMContentLoaded', function() {
    initializeAnimations();
});

// Initialize all animations
function initializeAnimations() {
    // Add fade-in animation to elements when they come into view
    addScrollAnimations();
    
    // Add hover effects to cards
    addCardAnimations();
    
    // Add button animations
    addButtonAnimations();
    
    // Add form animations
    addFormAnimations();
    
    // Add table animations
    addTableAnimations();
    
    // Add loading animations
    addLoadingAnimations();
    
    // Add page transition animations
    addPageTransitions();
}

// Scroll-triggered animations
function addScrollAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-fade-in');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Observe all cards, sections, and important elements
    document.querySelectorAll('.card, .section, .container, .row').forEach(el => {
        observer.observe(el);
    });
}

// Card hover animations
function addCardAnimations() {
    document.querySelectorAll('.card').forEach(card => {
        card.classList.add('card-animated');
        
        // Add staggered animation delay
        const cards = document.querySelectorAll('.card');
        cards.forEach((card, index) => {
            card.style.animationDelay = `${index * 0.1}s`;
        });
    });
}

// Button animations
function addButtonAnimations() {
    document.querySelectorAll('.btn').forEach(btn => {
        btn.classList.add('btn-animated');
        
        // Add click animation
        btn.addEventListener('click', function(e) {
            // Create ripple effect
            const ripple = document.createElement('span');
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;
            
            ripple.style.width = ripple.style.height = size + 'px';
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';
            ripple.classList.add('ripple');
            
            this.appendChild(ripple);
            
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    });
}

// Form animations
function addFormAnimations() {
    document.querySelectorAll('.form-control').forEach(input => {
        input.classList.add('form-control-animated');
        
        // Add floating label effect
        if (input.value) {
            input.parentElement.classList.add('has-value');
        }
        
        input.addEventListener('focus', function() {
            this.parentElement.classList.add('focused');
        });
        
        input.addEventListener('blur', function() {
            this.parentElement.classList.remove('focused');
            if (this.value) {
                this.parentElement.classList.add('has-value');
            } else {
                this.parentElement.classList.remove('has-value');
            }
        });
    });
}

// Table row animations
function addTableAnimations() {
    document.querySelectorAll('tbody tr').forEach((row, index) => {
        row.classList.add('table-row-animated');
        row.style.animationDelay = `${index * 0.05}s`;
    });
}

// Loading animations
function addLoadingAnimations() {
    // Show loading spinner for AJAX requests
    $(document).ajaxStart(function() {
        showLoadingSpinner();
    });
    
    $(document).ajaxStop(function() {
        hideLoadingSpinner();
    });
}

// Show loading spinner
function showLoadingSpinner() {
    const spinner = document.createElement('div');
    spinner.id = 'loading-spinner';
    spinner.className = 'loading-spinner';
    spinner.style.cssText = `
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        z-index: 9999;
        background: white;
        border-radius: 50%;
        padding: 20px;
        box-shadow: 0 4px 20px rgba(0,0,0,0.1);
    `;
    document.body.appendChild(spinner);
}

// Hide loading spinner
function hideLoadingSpinner() {
    const spinner = document.getElementById('loading-spinner');
    if (spinner) {
        spinner.remove();
    }
}

// Page transition animations
function addPageTransitions() {
    // Add page enter animation
    document.body.classList.add('page-enter');
    
    setTimeout(() => {
        document.body.classList.add('page-enter-active');
        document.body.classList.remove('page-enter');
    }, 100);
}

// Typing animation for text
function typeWriter(element, text, speed = 100) {
    let i = 0;
    element.innerHTML = '';
    
    function type() {
        if (i < text.length) {
            element.innerHTML += text.charAt(i);
            i++;
            setTimeout(type, speed);
        }
    }
    
    type();
}

// Parallax effect for background elements
function addParallaxEffect() {
    window.addEventListener('scroll', () => {
        const scrolled = window.pageYOffset;
        const parallaxElements = document.querySelectorAll('.parallax');
        
        parallaxElements.forEach(element => {
            const speed = element.dataset.speed || 0.5;
            element.style.transform = `translateY(${scrolled * speed}px)`;
        });
    });
}

// Counter animation
function animateCounter(element, target, duration = 2000) {
    let start = 0;
    const increment = target / (duration / 16);
    
    function updateCounter() {
        start += increment;
        if (start < target) {
            element.textContent = Math.floor(start);
            requestAnimationFrame(updateCounter);
        } else {
            element.textContent = target;
        }
    }
    
    updateCounter();
}

// Progress bar animation
function animateProgressBar(progressBar, targetPercentage) {
    progressBar.style.setProperty('--progress-width', targetPercentage + '%');
    progressBar.classList.add('progress-animated');
}

// Modal animations
function addModalAnimations() {
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        modal.classList.add('modal-animated');
    });
}

// Alert animations
function addAlertAnimations() {
    document.querySelectorAll('.alert').forEach(alert => {
        alert.classList.add('alert-animated');
        
        // Auto-dismiss after 5 seconds
        setTimeout(() => {
            alert.style.animation = 'slideOutRight 0.5s ease-in forwards';
            setTimeout(() => {
                alert.remove();
            }, 500);
        }, 5000);
    });
}

// Icon animations
function addIconAnimations() {
    document.querySelectorAll('.fas, .fa, .icon').forEach(icon => {
        icon.classList.add('icon-animated');
    });
}

// Navigation animations
function addNavigationAnimations() {
    document.querySelectorAll('.nav-link, .nav-item a').forEach(link => {
        link.classList.add('nav-item-animated');
    });
}

// Utility functions
function addClassWithDelay(element, className, delay) {
    setTimeout(() => {
        element.classList.add(className);
    }, delay);
}

function removeClassWithDelay(element, className, delay) {
    setTimeout(() => {
        element.classList.remove(className);
    }, delay);
}

// Export functions for global use
window.AnimationUtils = {
    typeWriter,
    animateCounter,
    animateProgressBar,
    showLoadingSpinner,
    hideLoadingSpinner,
    addClassWithDelay,
    removeClassWithDelay
};

// Add CSS for ripple effect
const rippleCSS = `
.ripple {
    position: absolute;
    border-radius: 50%;
    background: rgba(255, 255, 255, 0.6);
    transform: scale(0);
    animation: ripple-animation 0.6s linear;
    pointer-events: none;
}

@keyframes ripple-animation {
    to {
        transform: scale(4);
        opacity: 0;
    }
}
`;

// Inject ripple CSS
const style = document.createElement('style');
style.textContent = rippleCSS;
document.head.appendChild(style); 