function AmountCard({ label, amount, description }) {
  const formattedAmount = new Intl.NumberFormat("en-IN", {
    style: "currency",
    currency: "INR",
    maximumFractionDigits: 2,
  }).format(amount ?? 0);

  return (
    <article className="dashboard-amount-card">
      <p className="dashboard-amount-label">{label}</p>
      <p className="dashboard-amount-value">{formattedAmount}</p>
      {description && <p className="dashboard-amount-description">{description}</p>}
    </article>
  );
}

export default AmountCard;
