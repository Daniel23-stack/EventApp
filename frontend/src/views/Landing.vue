<template>
  <div class="landing-page">
    <!-- Navigation Bar -->
    <nav class="landing-nav" :class="{ 'scrolled': isScrolled }">
      <div class="nav-container">
        <div class="nav-brand">
          <span class="brand-icon">📦</span>
          <span class="brand-text">EvenApp</span>
        </div>
        <div class="nav-menu" :class="{ 'active': menuOpen }">
          <a href="#features" @click="scrollTo('features')" class="nav-link">Features</a>
          <a href="#technology" @click="scrollTo('technology')" class="nav-link">Technology</a>
          <a href="#about" @click="scrollTo('about')" class="nav-link">About</a>
          <router-link to="/login" class="nav-link">Sign In</router-link>
          <router-link to="/register" class="btn btn-nav-primary">Get Started</router-link>
        </div>
        <button class="menu-toggle" @click="menuOpen = !menuOpen">
          <span></span>
          <span></span>
          <span></span>
        </button>
      </div>
    </nav>

    <!-- Hero Section -->
    <section class="hero" id="home">
      <div class="hero-background">
        <div class="gradient-orb orb-1"></div>
        <div class="gradient-orb orb-2"></div>
        <div class="gradient-orb orb-3"></div>
      </div>
      <div class="hero-content">
        <div class="hero-badge">
          <span>✨ Smart Inventory Management</span>
        </div>
        <h1 class="hero-title">
          Take Control of Your
          <span class="gradient-text">Inventory</span>
        </h1>
        <p class="hero-subtitle">
          Streamline your inventory operations with real-time tracking, automated alerts, 
          and advanced analytics. Built for modern businesses.
        </p>
        <div class="hero-buttons">
          <router-link to="/register" class="btn btn-primary btn-large">
            <span>Start Free Trial</span>
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
              <path d="M7.5 15L12.5 10L7.5 5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
          </router-link>
          <router-link to="/login" class="btn btn-secondary btn-large">
            Sign In
          </router-link>
        </div>
        <div class="hero-stats">
          <div class="stat-item">
            <div class="stat-number">99.9%</div>
            <div class="stat-label">Uptime</div>
          </div>
          <div class="stat-item">
            <div class="stat-number">10K+</div>
            <div class="stat-label">Products Tracked</div>
          </div>
          <div class="stat-item">
            <div class="stat-number">24/7</div>
            <div class="stat-label">Support</div>
          </div>
        </div>
      </div>
      <div class="hero-visual">
        <div class="dashboard-preview">
          <div class="preview-header">
            <div class="preview-dot"></div>
            <div class="preview-dot"></div>
            <div class="preview-dot"></div>
          </div>
          <div class="preview-content">
            <div class="preview-card card-1">
              <div class="card-header">📊</div>
              <div class="card-title">Analytics</div>
              <div class="card-value">$125K</div>
            </div>
            <div class="preview-card card-2">
              <div class="card-header">📦</div>
              <div class="card-title">Inventory</div>
              <div class="card-value">1,234</div>
            </div>
            <div class="preview-card card-3">
              <div class="card-header">🔔</div>
              <div class="card-title">Alerts</div>
              <div class="card-value">12</div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features" id="features">
      <div class="container">
        <div class="section-header">
          <div class="section-badge">Features</div>
          <h2 class="section-title">Everything You Need</h2>
          <p class="section-subtitle">Powerful features to manage your inventory efficiently</p>
        </div>
        <div class="features-grid">
          <div class="feature-card" v-for="(feature, index) in features" :key="index">
            <div class="feature-icon-wrapper">
              <div class="feature-icon">{{ feature.icon }}</div>
            </div>
            <h3>{{ feature.title }}</h3>
            <p>{{ feature.description }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Technology Stack Section -->
    <section class="tech-stack" id="technology">
      <div class="container">
        <div class="section-header">
          <div class="section-badge">Technology</div>
          <h2 class="section-title">Built with Modern Stack</h2>
          <p class="section-subtitle">Leveraging the latest technologies for performance and reliability</p>
        </div>
        <div class="tech-grid">
          <div class="tech-card" v-for="(tech, index) in technologies" :key="index">
            <div class="tech-logo">{{ tech.icon }}</div>
            <h3>{{ tech.name }}</h3>
            <p>{{ tech.description }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- About/Stats Section -->
    <section class="about" id="about">
      <div class="container">
        <div class="about-content">
          <div class="about-text">
            <div class="section-badge">Why Choose Us</div>
            <h2 class="section-title">Trusted by Businesses Worldwide</h2>
            <p class="about-description">
              Our Smart Inventory Management System is designed with Clean Architecture and SOLID principles, 
              ensuring scalability, maintainability, and performance. Whether you're a small business or a 
              large enterprise, we've got you covered.
            </p>
            <div class="about-features">
              <div class="about-feature">
                <div class="check-icon">✓</div>
                <span>Real-time inventory tracking</span>
              </div>
              <div class="about-feature">
                <div class="check-icon">✓</div>
                <span>Automated restocking alerts</span>
              </div>
              <div class="about-feature">
                <div class="check-icon">✓</div>
                <span>Advanced analytics & forecasting</span>
              </div>
              <div class="about-feature">
                <div class="check-icon">✓</div>
                <span>Role-based access control</span>
              </div>
            </div>
          </div>
          <div class="about-visual">
            <div class="stats-box">
              <div class="stat-box-item">
                <div class="stat-box-number">100%</div>
                <div class="stat-box-label">Secure</div>
              </div>
              <div class="stat-box-item">
                <div class="stat-box-number">Fast</div>
                <div class="stat-box-label">Performance</div>
              </div>
              <div class="stat-box-item">
                <div class="stat-box-number">24/7</div>
                <div class="stat-box-label">Available</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="cta">
      <div class="container">
        <div class="cta-content">
          <h2>Ready to Transform Your Inventory Management?</h2>
          <p>Join thousands of businesses managing their inventory efficiently with EvenApp</p>
          <div class="cta-buttons">
            <router-link to="/register" class="btn btn-primary btn-large">
              Start Free Trial
            </router-link>
            <router-link to="/login" class="btn btn-outline btn-large">
              Sign In
            </router-link>
          </div>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer class="footer">
      <div class="container">
        <div class="footer-content">
          <div class="footer-brand">
            <div class="brand-icon">📦</div>
            <div class="brand-text">EvenApp</div>
            <p class="footer-description">Smart Inventory Management System</p>
          </div>
          <div class="footer-links">
            <div class="footer-column">
              <h4>Product</h4>
              <a href="#features">Features</a>
              <a href="#technology">Technology</a>
              <a href="#about">About</a>
            </div>
            <div class="footer-column">
              <h4>Resources</h4>
              <a href="#">Documentation</a>
              <a href="#">API Reference</a>
              <a href="#">Support</a>
            </div>
            <div class="footer-column">
              <h4>Company</h4>
              <a href="#">Blog</a>
              <a href="#">Careers</a>
              <a href="#">Contact</a>
            </div>
          </div>
        </div>
        <div class="footer-bottom">
          <p>&copy; 2024 EvenApp Inventory Management System. All rights reserved.</p>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

const menuOpen = ref(false);
const isScrolled = ref(false);

const features = [
  {
    icon: '🔐',
    title: 'Secure Authentication',
    description: 'JWT-based authentication with role-based access control for Admin, Manager, and User roles.'
  },
  {
    icon: '⚡',
    title: 'Real-Time Updates',
    description: 'Get instant inventory updates and alerts using SignalR WebSocket technology.'
  },
  {
    icon: '📈',
    title: 'Advanced Analytics',
    description: 'Sales forecasting, turnover rates, ABC analysis, and product performance metrics.'
  },
  {
    icon: '🔔',
    title: 'Automated Alerts',
    description: 'Automated restocking alerts when inventory levels drop below thresholds.'
  },
  {
    icon: '👥',
    title: 'Supplier Management',
    description: 'Manage suppliers, track orders, and maintain supplier-product relationships.'
  },
  {
    icon: '📋',
    title: 'Order Tracking',
    description: 'Complete order management system with status tracking from pending to delivered.'
  }
];

const technologies = [
  {
    icon: '.NET',
    name: '.NET 8',
    description: 'High-performance API backend'
  },
  {
    icon: 'Vue',
    name: 'Vue.js 3',
    description: 'Modern reactive frontend'
  },
  {
    icon: 'MySQL',
    name: 'MySQL',
    description: 'Reliable database system'
  },
  {
    icon: 'SignalR',
    name: 'SignalR',
    description: 'Real-time communication'
  },
  {
    icon: 'Dapper',
    name: 'Dapper',
    description: 'Fast data access'
  },
  {
    icon: 'Clean',
    name: 'Clean Architecture',
    description: 'SOLID principles'
  }
];

const scrollTo = (elementId) => {
  menuOpen.value = false;
  const element = document.getElementById(elementId);
  if (element) {
    element.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }
};

const handleScroll = () => {
  isScrolled.value = window.scrollY > 50;
};

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
});

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll);
});
</script>

<style scoped>
.landing-page {
  min-height: 100vh;
  background: #0a0e27;
  color: white;
  overflow-x: hidden;
}

/* Navigation */
.landing-nav {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: 1000;
  background: rgba(10, 14, 39, 0.8);
  backdrop-filter: blur(20px);
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  transition: all 0.3s ease;
}

.landing-nav.scrolled {
  background: rgba(10, 14, 39, 0.95);
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
}

.nav-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.nav-brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.5rem;
  font-weight: 700;
  text-decoration: none;
  color: white;
}

.brand-icon {
  font-size: 2rem;
}

.brand-text {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.nav-menu {
  display: flex;
  align-items: center;
  gap: 2rem;
}

.nav-link {
  color: rgba(255, 255, 255, 0.8);
  text-decoration: none;
  font-weight: 500;
  transition: color 0.3s ease;
  cursor: pointer;
}

.nav-link:hover {
  color: white;
}

.btn-nav-primary {
  padding: 0.75rem 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 8px;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.3s ease;
  border: none;
}

.btn-nav-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 25px rgba(102, 126, 234, 0.4);
}

.menu-toggle {
  display: none;
  flex-direction: column;
  gap: 5px;
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 0.5rem;
}

.menu-toggle span {
  width: 25px;
  height: 3px;
  background: white;
  border-radius: 2px;
  transition: all 0.3s ease;
}

/* Hero Section */
.hero {
  min-height: 100vh;
  display: flex;
  align-items: center;
  padding: 8rem 2rem 4rem;
  max-width: 1400px;
  margin: 0 auto;
  position: relative;
  overflow: hidden;
}

.hero-background {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  overflow: hidden;
  z-index: 0;
}

.gradient-orb {
  position: absolute;
  border-radius: 50%;
  filter: blur(80px);
  opacity: 0.3;
  animation: float 20s ease-in-out infinite;
}

.orb-1 {
  width: 500px;
  height: 500px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  top: -200px;
  left: -200px;
  animation-delay: 0s;
}

.orb-2 {
  width: 400px;
  height: 400px;
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
  bottom: -150px;
  right: -150px;
  animation-delay: 5s;
}

.orb-3 {
  width: 300px;
  height: 300px;
  background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
  top: 50%;
  right: 10%;
  animation-delay: 10s;
}

@keyframes float {
  0%, 100% {
    transform: translate(0, 0) scale(1);
  }
  33% {
    transform: translate(30px, -30px) scale(1.1);
  }
  66% {
    transform: translate(-20px, 20px) scale(0.9);
  }
}

.hero-content {
  flex: 1;
  max-width: 600px;
  z-index: 1;
  position: relative;
}

.hero-badge {
  display: inline-block;
  padding: 0.5rem 1rem;
  background: rgba(102, 126, 234, 0.2);
  border: 1px solid rgba(102, 126, 234, 0.3);
  border-radius: 50px;
  margin-bottom: 1.5rem;
  font-size: 0.9rem;
  color: rgba(255, 255, 255, 0.9);
}

.hero-title {
  font-size: 4.5rem;
  font-weight: 800;
  margin-bottom: 1.5rem;
  line-height: 1.1;
}

.gradient-text {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.hero-subtitle {
  font-size: 1.3rem;
  margin-bottom: 2.5rem;
  opacity: 0.8;
  line-height: 1.6;
  color: rgba(255, 255, 255, 0.9);
}

.hero-buttons {
  display: flex;
  gap: 1rem;
  margin-bottom: 3rem;
  flex-wrap: wrap;
}

.btn {
  padding: 1rem 2rem;
  border-radius: 12px;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.3s ease;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  border: 2px solid transparent;
  cursor: pointer;
}

.btn-large {
  padding: 1.25rem 2.5rem;
  font-size: 1.1rem;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(102, 126, 234, 0.6);
}

.btn-secondary {
  background: rgba(255, 255, 255, 0.1);
  color: white;
  border: 2px solid rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
}

.btn-secondary:hover {
  background: rgba(255, 255, 255, 0.15);
  border-color: rgba(255, 255, 255, 0.3);
  transform: translateY(-2px);
}

.btn-outline {
  background: transparent;
  color: white;
  border: 2px solid rgba(255, 255, 255, 0.3);
}

.btn-outline:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.5);
}

.hero-stats {
  display: flex;
  gap: 3rem;
  margin-top: 3rem;
}

.stat-item {
  text-align: center;
}

.stat-number {
  font-size: 2rem;
  font-weight: 700;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.stat-label {
  font-size: 0.9rem;
  opacity: 0.7;
  margin-top: 0.5rem;
}

/* Hero Visual */
.hero-visual {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  z-index: 1;
}

.dashboard-preview {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
  padding: 1.5rem;
  width: 400px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  animation: float 6s ease-in-out infinite;
}

.preview-header {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}

.preview-dot {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.3);
}

.preview-content {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
}

.preview-card {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  padding: 1.5rem;
  text-align: center;
  transition: all 0.3s ease;
}

.preview-card:hover {
  background: rgba(255, 255, 255, 0.1);
  transform: translateY(-5px);
}

.card-header {
  font-size: 2rem;
  margin-bottom: 0.5rem;
}

.card-title {
  font-size: 0.9rem;
  opacity: 0.7;
  margin-bottom: 0.5rem;
}

.card-value {
  font-size: 1.5rem;
  font-weight: 700;
}

.card-3 {
  grid-column: 1 / -1;
}

/* Features Section */
.features {
  padding: 8rem 2rem;
  background: rgba(255, 255, 255, 0.02);
  position: relative;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
}

.section-header {
  text-align: center;
  margin-bottom: 4rem;
}

.section-badge {
  display: inline-block;
  padding: 0.5rem 1rem;
  background: rgba(102, 126, 234, 0.2);
  border: 1px solid rgba(102, 126, 234, 0.3);
  border-radius: 50px;
  margin-bottom: 1rem;
  font-size: 0.9rem;
  color: rgba(255, 255, 255, 0.9);
}

.section-title {
  font-size: 3rem;
  font-weight: 700;
  margin-bottom: 1rem;
}

.section-subtitle {
  font-size: 1.2rem;
  opacity: 0.7;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
}

.feature-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
  padding: 2.5rem;
  text-align: center;
  transition: all 0.3s ease;
}

.feature-card:hover {
  transform: translateY(-10px);
  background: rgba(255, 255, 255, 0.08);
  border-color: rgba(102, 126, 234, 0.5);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.3);
}

.feature-icon-wrapper {
  margin-bottom: 1.5rem;
}

.feature-icon {
  font-size: 4rem;
  display: inline-block;
  animation: float 6s ease-in-out infinite;
}

.feature-card h3 {
  font-size: 1.5rem;
  margin-bottom: 1rem;
  font-weight: 600;
}

.feature-card p {
  opacity: 0.8;
  line-height: 1.6;
  color: rgba(255, 255, 255, 0.9);
}

/* Tech Stack Section */
.tech-stack {
  padding: 8rem 2rem;
  position: relative;
}

.tech-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}

.tech-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 16px;
  padding: 2.5rem;
  text-align: center;
  transition: all 0.3s ease;
}

.tech-card:hover {
  transform: translateY(-5px);
  background: rgba(255, 255, 255, 0.08);
  border-color: rgba(102, 126, 234, 0.5);
}

.tech-logo {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.tech-card h3 {
  font-size: 1.3rem;
  margin-bottom: 0.5rem;
  font-weight: 600;
}

.tech-card p {
  opacity: 0.7;
  font-size: 0.9rem;
}

/* About Section */
.about {
  padding: 8rem 2rem;
  background: rgba(255, 255, 255, 0.02);
}

.about-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 4rem;
  align-items: center;
}

.about-description {
  font-size: 1.1rem;
  line-height: 1.8;
  opacity: 0.8;
  margin-bottom: 2rem;
  color: rgba(255, 255, 255, 0.9);
}

.about-features {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.about-feature {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.check-icon {
  width: 24px;
  height: 24px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.9rem;
}

.about-visual {
  display: flex;
  justify-content: center;
}

.stats-box {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
  padding: 3rem;
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 2rem;
  width: 100%;
}

.stat-box-item {
  text-align: center;
}

.stat-box-number {
  font-size: 2.5rem;
  font-weight: 700;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 0.5rem;
}

.stat-box-label {
  opacity: 0.7;
  font-size: 0.9rem;
}

/* CTA Section */
.cta {
  padding: 8rem 2rem;
  text-align: center;
  background: linear-gradient(135deg, rgba(102, 126, 234, 0.1) 0%, rgba(118, 75, 162, 0.1) 100%);
  position: relative;
}

.cta-content h2 {
  font-size: 3rem;
  margin-bottom: 1rem;
  font-weight: 700;
}

.cta-content p {
  font-size: 1.3rem;
  margin-bottom: 2.5rem;
  opacity: 0.8;
}

.cta-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
  flex-wrap: wrap;
}

/* Footer */
.footer {
  padding: 4rem 2rem 2rem;
  background: rgba(0, 0, 0, 0.3);
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.footer-content {
  display: grid;
  grid-template-columns: 2fr 3fr;
  gap: 4rem;
  margin-bottom: 3rem;
}

.footer-brand {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.footer-description {
  opacity: 0.7;
  margin-top: 0.5rem;
}

.footer-links {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 2rem;
}

.footer-column h4 {
  margin-bottom: 1rem;
  font-weight: 600;
}

.footer-column a {
  display: block;
  color: rgba(255, 255, 255, 0.7);
  text-decoration: none;
  margin-bottom: 0.5rem;
  transition: color 0.3s ease;
}

.footer-column a:hover {
  color: white;
}

.footer-bottom {
  text-align: center;
  padding-top: 2rem;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  opacity: 0.6;
}

/* Responsive Design */
@media (max-width: 968px) {
  .menu-toggle {
    display: flex;
  }

  .nav-menu {
    position: fixed;
    top: 70px;
    left: 0;
    right: 0;
    background: rgba(10, 14, 39, 0.98);
    backdrop-filter: blur(20px);
    flex-direction: column;
    padding: 2rem;
    transform: translateX(-100%);
    transition: transform 0.3s ease;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
  }

  .nav-menu.active {
    transform: translateX(0);
  }

  .hero {
    flex-direction: column;
    text-align: center;
    padding: 6rem 1rem 4rem;
  }

  .hero-title {
    font-size: 2.5rem;
  }

  .hero-subtitle {
    font-size: 1.1rem;
  }

  .hero-visual {
    margin-top: 3rem;
  }

  .dashboard-preview {
    width: 100%;
    max-width: 400px;
  }

  .hero-stats {
    justify-content: center;
    gap: 2rem;
  }

  .section-title {
    font-size: 2rem;
  }

  .features-grid,
  .tech-grid {
    grid-template-columns: 1fr;
  }

  .about-content {
    grid-template-columns: 1fr;
  }

  .footer-content {
    grid-template-columns: 1fr;
  }

  .footer-links {
    grid-template-columns: 1fr;
  }
}
</style>
