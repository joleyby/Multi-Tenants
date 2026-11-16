namespace Presentacion;

partial class FrmPrincipal
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private MenuStrip menuPrincipal;
    private ToolStripMenuItem menuArchivo;
    private ToolStripMenuItem menuSalir;
    private ToolStripMenuItem menuEstablecimientos;
    private ToolStripMenuItem menuConsultaEstablecimientos;
    private ToolStripMenuItem menuCatalogos;
    private ToolStripMenuItem menuProvincias;
    private ToolStripMenuItem menuSeguridad;
    private ToolStripMenuItem menuUsuarios;
    private ToolStripMenuItem menuBitacora;
    private ToolStripMenuItem menuCuenta;
    private ToolStripMenuItem menuCambiarContraseña;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblUsuario;
    private ToolStripMenuItem menuCerrarSesion;
    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        menuPrincipal = new MenuStrip();
        menuArchivo = new ToolStripMenuItem();
        menuSalir = new ToolStripMenuItem();
        menuEstablecimientos = new ToolStripMenuItem();
        menuConsultaEstablecimientos = new ToolStripMenuItem();
        menuCatalogos = new ToolStripMenuItem();
        menuProvincias = new ToolStripMenuItem();
        menuSeguridad = new ToolStripMenuItem();
        menuUsuarios = new ToolStripMenuItem();
        menuBitacora = new ToolStripMenuItem();
        menuCuenta = new ToolStripMenuItem();
        menuCambiarContraseña = new ToolStripMenuItem();
        barraEstado = new StatusStrip();
        lblUsuario = new ToolStripStatusLabel();
        menuCerrarSesion = new ToolStripMenuItem();
        cerrarSesionToolStripMenuItem = new ToolStripMenuItem();
        menuPrincipal.SuspendLayout();
        barraEstado.SuspendLayout();
        SuspendLayout();
        // 
        // menuPrincipal
        // 
        menuPrincipal.ImageScalingSize = new Size(20, 20);
        menuPrincipal.Items.AddRange(new ToolStripItem[] { menuEstablecimientos, menuCatalogos, menuSeguridad, menuCuenta, menuArchivo });
        menuPrincipal.Location = new Point(0, 0);
        menuPrincipal.Name = "menuPrincipal";
        menuPrincipal.Size = new Size(1000, 28);
        menuPrincipal.TabIndex = 0;
        // 
        // menuArchivo
        // 
        menuArchivo.DropDownItems.AddRange(new ToolStripItem[] { menuCerrarSesion, menuSalir });
        menuArchivo.Name = "menuArchivo";
        menuArchivo.Size = new Size(73, 24);
        menuArchivo.Text = "&Archivo";
        // 
        // menuCerrarSesion
        // 
        menuCerrarSesion.Name = "menuCerrarSesion";
        menuCerrarSesion.Size = new Size(180, 26);
        menuCerrarSesion.Text = "&Cerrar sesión";
        menuCerrarSesion.Click += menuCerrarSesion_Click;
        // 
        // menuSalir
        // 
        menuSalir.Name = "menuSalir";
        menuSalir.Size = new Size(224, 26);
        menuSalir.Text = "&Salir";
        menuSalir.Click += menuSalir_Click;
        // 
        // menuEstablecimientos
        // 
        menuEstablecimientos.DropDownItems.AddRange(new ToolStripItem[] { menuConsultaEstablecimientos });
        menuEstablecimientos.Name = "menuEstablecimientos";
        menuEstablecimientos.Size = new Size(135, 24);
        menuEstablecimientos.Text = "&Establecimientos";
        // 
        // menuConsultaEstablecimientos
        // 
        menuConsultaEstablecimientos.Name = "menuConsultaEstablecimientos";
        menuConsultaEstablecimientos.Size = new Size(231, 26);
        menuConsultaEstablecimientos.Text = "&Gestionar / Consultar";
        menuConsultaEstablecimientos.Click += menuConsultaEstablecimientos_Click;
        // 
        // menuCatalogos
        // 
        menuCatalogos.DropDownItems.AddRange(new ToolStripItem[] { menuProvincias });
        menuCatalogos.Name = "menuCatalogos";
        menuCatalogos.Size = new Size(90, 24);
        menuCatalogos.Text = "&Catálogos";
        // 
        // menuProvincias
        // 
        menuProvincias.Name = "menuProvincias";
        menuProvincias.Size = new Size(158, 26);
        menuProvincias.Text = "&Provincias";
        menuProvincias.Click += menuProvincias_Click;
        // 
        // menuSeguridad
        // 
        menuSeguridad.DropDownItems.AddRange(new ToolStripItem[] { menuUsuarios, menuBitacora });
        menuSeguridad.Name = "menuSeguridad";
        menuSeguridad.Size = new Size(91, 24);
        menuSeguridad.Text = "&Seguridad";
        // 
        // menuUsuarios
        // 
        menuUsuarios.Name = "menuUsuarios";
        menuUsuarios.Size = new Size(148, 26);
        menuUsuarios.Text = "&Usuarios";
        menuUsuarios.Click += menuUsuarios_Click;
        // 
        // menuBitacora
        // 
        menuBitacora.Name = "menuBitacora";
        menuBitacora.Size = new Size(148, 26);
        menuBitacora.Text = "&Bitácora";
        menuBitacora.Click += menuBitacora_Click;
        // 
        // menuCuenta
        // 
        menuCuenta.DropDownItems.AddRange(new ToolStripItem[] { menuCambiarContraseña });
        menuCuenta.Name = "menuCuenta";
        menuCuenta.Size = new Size(69, 24);
        menuCuenta.Text = "&Cuenta";
        // 
        // menuCambiarContraseña
        // 
        menuCambiarContraseña.Name = "menuCambiarContraseña";
        menuCambiarContraseña.Size = new Size(245, 26);
        menuCambiarContraseña.Text = "&Cambiar mi contraseña";
        menuCambiarContraseña.Click += menuCambiarContraseña_Click;
        // 
        // barraEstado
        // 
        barraEstado.ImageScalingSize = new Size(20, 20);
        barraEstado.Items.AddRange(new ToolStripItem[] { lblUsuario });
        barraEstado.Location = new Point(0, 432);
        barraEstado.Name = "barraEstado";
        barraEstado.Size = new Size(1000, 22);
        barraEstado.TabIndex = 1;
        // 
        // lblUsuario
        // 
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(0, 16);
        // 
        // cerrarSesionToolStripMenuItem
        // 
        cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
        cerrarSesionToolStripMenuItem.Size = new Size(224, 26);
        cerrarSesionToolStripMenuItem.Text = "Cerrar Sesion";
        cerrarSesionToolStripMenuItem.Click += menuCerrarSesion_Click;
        // 
        // FrmPrincipal
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 454);
        Controls.Add(barraEstado);
        Controls.Add(menuPrincipal);
        IsMdiContainer = true;
        MainMenuStrip = menuPrincipal;
        Name = "FrmPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Salud Nacional";
        menuPrincipal.ResumeLayout(false);
        menuPrincipal.PerformLayout();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ToolStripMenuItem cerrarSesionToolStripMenuItem;
}
