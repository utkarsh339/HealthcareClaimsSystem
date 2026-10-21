import { Link } from "react-router-dom";

const capabilities = [
  {
    number: "01",
    title: "Claims management",
    description:
      "Review claims, follow their processing status, and keep claim information organized in one place.",
  },
  {
    number: "02",
    title: "Patient & provider records",
    description:
      "Access patient and healthcare-provider information through dedicated management screens.",
  },
  {
    number: "03",
    title: "Operational visibility",
    description:
      "Use dashboard summaries, payment information, and audit logs to understand system activity.",
  },
];

function Home() {
  return (
    <main className="landing-page">
      <header className="landing-header">
        <Link className="landing-brand" to="/" aria-label="Healthcare Claims System home">
          <span className="landing-brand-mark" aria-hidden="true">
            <svg viewBox="0 0 32 32" fill="none">
              <path d="M16 4.5v23M4.5 16h23" stroke="currentColor" strokeWidth="4" strokeLinecap="round" />
              <path d="M23.8 7.8c-2.4-2.4-6.2-2.4-8.6 0L16 8.6l.8-.8c2.4-2.4 6.2-2.4 8.6 0 2.4 2.4 2.4 6.2 0 8.6L16 25.8l-9.4-9.4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" opacity=".35" />
            </svg>
          </span>
          <span>
            <strong>CareFlow</strong>
            <small>HEALTHCARE CLAIMS SYSTEM</small>
          </span>
        </Link>
        <nav className="landing-nav" aria-label="Main navigation">
          <a href="#platform">Platform</a>
          <a href="#capabilities">Capabilities</a>
          <Link className="landing-nav-login" to="/login">Log in <span aria-hidden="true">↗</span></Link>
        </nav>
      </header>

      <section className="landing-hero" id="platform">
        <div className="landing-hero-copy">
          <div className="landing-eyebrow"><span /> HEALTHCARE OPERATIONS, CONNECTED</div>
          <h1>Claims workflows.<br /><span>Clearer by design.</span></h1>
          <p className="landing-hero-description">
            A centralized workspace for managing healthcare claims, patient and provider records,
            insurance policies, prior authorizations, and payments.
          </p>
          <div className="landing-actions">
            <Link className="landing-button landing-button-primary" to="/login">
              Access your workspace <span aria-hidden="true">→</span>
            </Link>
            <Link className="landing-button landing-button-secondary" to="/register">
              Create an account
            </Link>
          </div>
          <div className="landing-assurance">
            <span className="landing-assurance-icon" aria-hidden="true">✓</span>
            <span>Sign in to access your authorized workspace</span>
          </div>
        </div>

        <div className="landing-visual" aria-label="Illustration of a claims operations dashboard">
          <div className="landing-orbit landing-orbit-one" />
          <div className="landing-orbit landing-orbit-two" />
          <div className="landing-dashboard-card">
            <div className="landing-preview-top">
              <div>
                <span className="landing-preview-kicker">WORKSPACE OVERVIEW</span>
                <h2>Claims overview</h2>
              </div>
              <span className="landing-preview-dots" aria-hidden="true">•••</span>
            </div>
            <div className="landing-preview-stats">
              <div className="landing-stat">
                <span className="landing-stat-icon landing-stat-purple">▤</span>
                <span className="landing-stat-label">Claims</span>
                <strong>Track</strong>
                <small>Processing activity</small>
              </div>
              <div className="landing-stat">
                <span className="landing-stat-icon landing-stat-green">✓</span>
                <span className="landing-stat-label">Payments</span>
                <strong>Review</strong>
                <small>Payment information</small>
              </div>
            </div>
            <div className="landing-preview-list-heading">
              <strong>Workspace modules</strong><span>QUICK VIEW</span>
            </div>
            <div className="landing-preview-row"><span className="landing-row-symbol">▦</span><span>Claims management</span><span className="landing-row-arrow">↗</span></div>
            <div className="landing-preview-row"><span className="landing-row-symbol landing-row-blue">♙</span><span>Patients & providers</span><span className="landing-row-arrow">↗</span></div>
            <div className="landing-preview-row"><span className="landing-row-symbol landing-row-green">◷</span><span>Audit & activity</span><span className="landing-row-arrow">↗</span></div>
            <div className="landing-preview-footer"><span className="landing-status-dot" /> Workspace access is protected</div>
          </div>
          <div className="landing-floating-card">
            <span className="landing-floating-check">✓</span>
            <span><strong>One connected workspace</strong><small>Claims · Records · Payments</small></span>
          </div>
        </div>
      </section>

      <section className="landing-capabilities" id="capabilities">
        <div className="landing-section-heading">
          <div>
            <span className="landing-eyebrow">BUILT FOR THE WORKFLOW</span>
            <h2>Everything in one place.</h2>
          </div>
          <p>Navigate core claims operations with focused tools for everyday healthcare administration.</p>
        </div>
        <div className="landing-capability-grid">
          {capabilities.map((item) => (
            <article className="landing-capability-card" key={item.number}>
              <span className="landing-capability-number">{item.number}</span>
              <h3>{item.title}</h3>
              <p>{item.description}</p>
              <Link to="/login" aria-label={`Log in to access ${item.title}`}>Explore workspace <span aria-hidden="true">→</span></Link>
            </article>
          ))}
        </div>
      </section>

      <footer className="landing-footer">
        <Link className="landing-footer-brand" to="/">CareFlow</Link>
        <span>Healthcare Claims System</span>
        <div><Link to="/login">Log in</Link><Link to="/register">Create account</Link></div>
      </footer>
    </main>
  );
}

export default Home;
