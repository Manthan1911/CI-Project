const root = document.querySelector(":root");
const scrollableArea = document.getElementById("scrollable-area");
const header = document.getElementById("header");

const headerProfile = document.getElementById("header-profile");
const headerProfileHeight = +getComputedStyle(root)
  .getPropertyValue("--header-height")
  .slice(0, -2);

const filterMissionNavbarHeight = getComputedStyle(filterMissionNavbar).height;

const observer = new ResizeObserver((entries) => {
  const isFilterMissionOpen =
    entries[0].contentRect.height > headerProfileHeight;

  if (isFilterMissionOpen) {
    scrollableArea.style.height = "calc(100% - calc(var(--header-height) * 3))";
  } else {
    scrollableArea.style.height = "calc(100% - calc(var(--header-height) * 2))";
  }
});

observer.observe(header);
